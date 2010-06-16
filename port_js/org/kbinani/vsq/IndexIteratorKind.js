/*
 * IndexItertorKind.js
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.IndexIteratorKind == undefined ){

    org.kbinani.vsq.IndexIteratorKind = {};

    org.kbinani.vsq.IndexIteratorKind.SINGER = 1 << 0;
    org.kbinani.vsq.IndexIteratorKind.NOTE = 1 << 1;
    org.kbinani.vsq.IndexIteratorKind.CRESCEND = 1 << 2;
    org.kbinani.vsq.IndexIteratorKind.DECRESCEND = 1 << 3;
    org.kbinani.vsq.IndexIteratorKind.DYNAFF = 1 << 4;

}
