/**
 * TimesigList.h
 * Copyright © 2012 kbinani
 *
 * This file is part of libvsq.
 *
 * libvsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * libvsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include <math.h>
#include <algorithm>
#include "TimesigList.h"

VSQ_BEGIN_NAMESPACE

using namespace std;

TimesigList::TimesigList()
{
    this->listSize = 0;
    this->list = NULL;
    this->updated = false;
}

TimesigList::~TimesigList()
{
    if( this->list ){
        this->listSize = 0;
        for( int i = 0; i < this->listSize; i++ ){
            Timesig *item = this->list[i];
            delete item;
        }
        free( this->list );
        this->list = NULL;
    }
}

void TimesigList::updateTimesigInfo()
{
    if( 0 < this->listSize ){
        qsort( this->list, this->listSize, sizeof( Timesig * ), Timesig::compare );

        int count = this->listSize;
        for( int j = 1; j < count; j++ ){
            Timesig *item = this->list[j - 1];
            int numerator = item->numerator;
            int denominator = item->denominator;
            tick_t clock = item->clock;
            int bar_count = item->barCount;
            int diff = (int)::floor( (double)(480 * 4 / denominator * numerator) );
            clock = clock + (this->list[j]->barCount - bar_count) * diff;
            this->list[j]->clock = clock;
        }

        this->updated = true;
    }
}

void TimesigList::push( Timesig item )
{
    int index = -1;
    for( int i = 0; i < listSize; i++ ){
        if( list[i]->barCount == item.barCount ){
            index = i;
            break;
        }
    }

    if( 0 <= index ){
        list[index]->barCount = item.barCount;
        list[index]->numerator = item.numerator;
        list[index]->denominator = item.denominator;
    }else{
        Timesig *add = new Timesig( item.numerator, item.denominator, item.barCount );
        add->clock = item.clock;
        this->list = (Timesig **)realloc( this->list, sizeof( Timesig * ) * (this->listSize + 1) );
        this->list[this->listSize] = add;
        this->listSize++;
    }
    this->updated = false;
}

Timesig TimesigList::get( int index )
{
    Timesig *item = this->list[index];
    Timesig result( item->numerator, item->denominator, item->barCount );
    result.clock = item->clock;
    return result;
}

int TimesigList::size()
{
    return this->listSize;
}

bool TimesigList::isUpdated()
{
    return this->updated;
}

Timesig TimesigList::getTimesigAt( tick_t clock )
{
    if( !updated ){
        updateTimesigInfo();
    }
    Timesig ret;
    ret.numerator = 4;
    ret.denominator = 4;
    ret.barCount = 0;

    int index = -1;
    if( 0 < listSize ){
        for( int i = listSize - 1; i >= 0; i-- ){
            index = i;
            if( list[i]->clock <= clock ){
                break;
            }
        }
    }

    if( 0 <= index ){
        ret.numerator = list[index]->numerator;
        ret.denominator = list[index]->denominator;
        int tickPerBar = 480 * 4 / ret.denominator * ret.numerator;
        int deltaBar = (int)::floor( (double)((clock - list[index]->clock) / tickPerBar) );
        ret.barCount = list[index]->barCount + deltaBar;
    }else{
        int tickPerBar = 480 * 4 / ret.denominator * ret.numerator;
        int deltaBar = (int)::floor( (double)(clock / tickPerBar) );
        ret.barCount = deltaBar;
    }

    return ret;
}

tick_t TimesigList::getClockFromBarCount( int barCount )
{
    if( !updated ){
        updateTimesigInfo();
    }
    int index = 0;
    for( int i = listSize - 1; i >= 0; i-- ){
        index = i;
        if( list[i]->barCount <= barCount ){
            break;
        }
    }
    Timesig *item = list[index];
    int numerator = item->numerator;
    int denominator = item->denominator;
    tick_t initClock = item->clock;
    int initBarCount = item->barCount;
    int clockPerBar = numerator * 480 * 4 / denominator;
    return initClock + (barCount - initBarCount) * clockPerBar;
}

VSQ_END_NAMESPACE
