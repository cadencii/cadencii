/**
 * MeasureLineIterator.cpp
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
#include <iostream>
#include "MeasureLineIterator.h"

VSQ_BEGIN_NAMESPACE

using namespace std;

MeasureLineIterator::MeasureLineIterator( TimesigList *list, tick_t end_clock )
{
    m_list = list;
    m_end_clock = end_clock;
    i = 0;
    t_end = -1;
    clock = 0;
    this->reset( end_clock );
}

bool MeasureLineIterator::hasNext()
{
    if( clock <= m_end_clock ){
        return true;
    }else{
        return false;
    }
}

MeasureLine MeasureLineIterator::next()
{
    int mod = clock_step * local_numerator;
    if( clock < t_end ){
        if( (clock - local_clock) % mod == 0 ){
            bar_counter++;
            MeasureLine ret;
            ret.tick = clock;
            ret.isBorder = true;
            clock += clock_step;
            return ret;
        }else{
            MeasureLine ret;
            ret.tick = clock;
            ret.isBorder = false;
            clock += clock_step;
            return ret;
        }
    }

    if( i < m_list->size() ){
        local_denominator = m_list->get( i ).denominator;
        local_numerator = m_list->get( i ).numerator;
        local_clock = m_list->get( i ).clock;
        int local_bar_count = m_list->get( i ).barCount;
        int denom = local_denominator;
        if( denom <= 0 ){
            denom = 4;
        }
        clock_step = 480 * 4 / denom;
        mod = clock_step * local_numerator;
        bar_counter = local_bar_count - 1;
        t_end = m_end_clock;
        if( i + 1 < m_list->size() ){
            t_end = m_list->get( i + 1 ).clock;
        }
        i++;
        clock = local_clock;
        if( clock < t_end ){
            if( (clock - local_clock) % mod == 0 ){
                bar_counter++;
                MeasureLine ret;
                ret.tick = clock;
                ret.isBorder = true;
                clock += clock_step;
                return ret;
            }else{
                MeasureLine ret;
                ret.tick = clock;
                ret.isBorder = false;
                clock += clock_step;
                return ret;
            }
        }
    }else{
        if( (clock - local_clock) % mod == 0 ){
            bar_counter++;
            MeasureLine ret;
            ret.tick = clock;
            ret.isBorder = true;
            clock += clock_step;
            return ret;
        }else{
            MeasureLine ret;
            ret.tick = clock;
            ret.isBorder = false;
            clock += clock_step;
            return ret;
        }
    }
    return MeasureLine();
}

void MeasureLineIterator::reset( tick_t end_clock )
{
    this->m_end_clock = end_clock;
    this->i = 0;
    this->t_end = -1;
    this->clock = 0;
    this->local_denominator = 0;
    this->local_numerator = 0;
    this->clock_step = 0;
    this->local_clock = 0;
    this->bar_counter = 0;
}

VSQ_END_NAMESPACE
