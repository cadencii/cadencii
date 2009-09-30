/*
 * ICommand.cs
 * Copyright (c) 2009 kbinani
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
using System.Collections.Generic;

using bocoree;

namespace Boare.Cadencii {

    public interface ICommand {
        /// <summary>
        /// 子コマンドのリスト
        /// </summary>
        Vector<ICommand> Child {
            get;
        }

        /// <summary>
        /// 親コマンドへの参照
        /// </summary>
        ICommand parent {
            get;
            set;
        }
    }

}
