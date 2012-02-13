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
#include <iostream>
#include "vsq.h"
#include "TimesigList.h"

VSQ_BEGIN_NAMESPACE

using namespace std;

void TimesigList::updateTimesigInfo()
{
    std::sort( this->list.begin(), this->list.end(), Timesig::compare );

    int count = this->list.size();
    for( int j = 1; j < count; j++ ){
        Timesig item = this->list[j - 1];
        int numerator = item.numerator;
        int denominator = item.denominator;
        tick_t clock = item.clock;
        int bar_count = item.barCount;
        int diff = ::floor( 480 * 4 / denominator * numerator );
        clock = clock + (this->list[j].barCount - bar_count) * diff;
        this->list[j].clock = clock;
    }
}

void TimesigList::push( Timesig item )
{
    this->list.push_back( item );
}

Timesig TimesigList::get( int index )
{
    return this->list[index];
}
VSQ_END_NAMESPACE
