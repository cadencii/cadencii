#if ENABLE_SCRIPT
/*
 * ScriptReturnStatus.cs
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
namespace org.kbinani.cadencii {

    public enum ScriptReturnStatus {
        /// <summary>
        /// スクリプトの実行が成功し、編集が行われた。
        /// </summary>
        EDITED,
        /// <summary>
        /// スクリプトの実行が成功したが、編集は行われなかった（あるいは編集の必要は無かった、等）
        /// </summary>
        NOT_EDITED,
        /// <summary>
        /// スクリプトの実行が失敗した、または中断された
        /// </summary>
        ERROR,
    }

}
#endif
