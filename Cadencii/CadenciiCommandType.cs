/*
 * CadenciiCommandType.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace Boare.Cadencii {

    public enum CadenciiCommandType {
        VSQ_COMMAND,
        BEZIER_CHAIN_ADD,
        BEZIER_CHAIN_DELETE,
        BEZIER_CHAIN_REPLACE,
        REPLACE,
        ATTACHED_CURVE_REPLACE_RANGE,
        TRACK_ADD,
        TRACK_DELETE,
        TRACK_EDIT_CURVE,
        TRACK_REPLACE,
        BGM_UPDATE,
    }

}
