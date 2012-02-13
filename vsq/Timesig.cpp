/**
 * Timesig.cpp
 * Copyright © 2012 kbinani
 *
 * This file is part of vsq.
 *
 * vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include <sstream>
#include "Timesig.h"

VSQ_BEGIN_NAMESPACE

using namespace std;

Timesig::Timesig( int numerator, int denominator, int barCount )
{
    this->clock = 0;
    this->numerator = numerator;
    this->denominator = denominator;
    this->barCount = barCount;
}

Timesig::Timesig()
{
    this->clock = 0;
    this->numerator = 4;
    this->denominator = 4;
    this->barCount = 0;
}

const string Timesig::toString()
{
    ostringstream oss;
    oss << "{Clock=" << this->clock << ", Numerator=" << this->numerator << ", Denominator=" << this->denominator << ", BarCount=" << this->barCount << "}";
    return oss.str();
}

VSQ_END_NAMESPACE

