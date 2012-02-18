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
#ifndef __TimesigList_h__
#define __TimesigList_h__

#include <vector>
#include "Timesig.h"

VSQ_BEGIN_NAMESPACE

/**
 * @brief 拍子情報を格納したテーブルを表すクラス
 */
class TimesigList{
private:
    /**
     * @brief 拍子情報を格納したリスト。tick の昇順で格納する
     */
    Timesig **list;

    /**
     * @brief リストのサイズ
     */
    int listSize;

    /**
     * @brief updateTimesigInfo メソッドが呼ばれたかどうか
     */
    bool updated;

public:
    TimesigList();

    ~TimesigList();

    /**
     * @brief 指定したインデックスの拍子変更情報を取得する
     * @param index 取得するデータ点のインデックス(0から始まる)
     * @return 拍子変更情報
     */
    Timesig get( int index );

    /**
     * @brief リスト内の拍子変更情報の clock の部分を更新する
     */
    void updateTimesigInfo();

    /**
     * @brief データ点を追加する
     * @param item 追加する拍子変更情報
     */
    void push( Timesig item );

    /**
     * @brief データ点の個数を返す
     */
    int size();

    /**
     * @brief updateTimesigInfo メソッドが呼ばれたかどうかを取得する
     */
    bool isUpdated();

    /**
     * @brief 指定された時刻における拍子情報を取得する
     * @param clock Tick 単位の時刻
     * @return 指定された時刻での拍子情報
     */
    Timesig getTimesigAt( tick_t tick );
};

VSQ_END_NAMESPACE

#endif
