/**
 * MeasureLineIterator.h
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
#ifndef __MeasureLineIterator_h__
#define __MeasureLineIterator_h__

#include "vsqglobal.h"
#include "TimesigList.h"
#include "MeasureLine.h"

VSQ_BEGIN_NAMESPACE

/**
 * @brief 小節を区切る線の情報を順に返す反復子
 */
class MeasureLineIterator
{
private:
    TimesigList *m_list;
    int m_end_clock;
    int i;
    tick_t clock;
    int local_denominator;
    int local_numerator;
    tick_t clock_step;
    int t_end;
    tick_t local_clock;
    int bar_counter;

public:
    /**
     * @brief 小節線の情報を取得する区間を指定し、初期化する
     * @param list テンポ変更リスト
     * @param end tick 単位で表した、区間の終了時刻
     */
    MeasureLineIterator( TimesigList *list, tick_t end );

    /**
     * @brief 次の小節線が取得可能かどうかを取得する
     * @return 取得可能であれば true を返す
     */
    bool hasNext();

    /**
     * @brief 次の小節線を取得する
     * @return 次の小節線の情報
     */
    MeasureLine next();

private:
    void reset( tick_t endTick );
};

VSQ_END_NAMESPACE

#endif
