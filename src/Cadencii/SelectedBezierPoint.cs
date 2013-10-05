/*
 * SelectedBezierPoint.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii
{

    public class SelectedBezierPoint
    {
        public int chainID = -1;
        public int pointID = -1;
        public BezierPickedSide picked;
        public BezierPoint original;

        public SelectedBezierPoint()
        {
            chainID = -1;
            pointID = -1;
        }

        public SelectedBezierPoint(int chain_id, int point_id, BezierPickedSide picked_side_, BezierPoint original_)
        {
            chainID = chain_id;
            pointID = point_id;
            picked = picked_side_;
            original = (BezierPoint)original_.clone();
        }
    }

}
