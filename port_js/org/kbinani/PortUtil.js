/*
 * PortUtil.js
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.PortUtil == undefined ) org.kbinani.PortUtil = {};

org.kbinani.PortUtil.s_ctrl = false;
org.kbinani.PortUtil.s_shift = false;
org.kbinani.PortUtil.s_alt = false;
org.kbinani.PortUtil.s_instance = null;

/**
 * overload1
 * sort( array )
 *
 * overload2
 * sort( array, compare_function )
 *
 * overload3
 * sort( array, start_index, length )
 *
 * overload4
 * sort( array, start_index, length, compare_function )
 */
org.kbinani.PortUtil.sort = function(){
    var array = arguments[0];
    var c = arguments.length;
    if( c == 1 ){
        array.sort();
    }else if( c == 2 ){
        var compare_function = arguments[1];
        array.sort( compare_function );
    }else if( c == 3 || c == 4 ){
        var start_index = arguments[1];
        var length = arguments[2];
        var end_index = start_index + length;
        if( c == 3 ){
            var changed = 1;
            while( changed > 0 ){
                changed = 0;
                for( var i = start_index; i < end_index - 1; i++ ){
                    for( var j = i + 1; j < end_index; j++ ){
                        if( array[i] > array[j] ){
                            var itemi = array[i];
                            array[i] = array[j];
                            array[j] = itemi;
                            changed++;
                        }
                    }
                }
            }
        }else{
            var changed = 1;
            var compare_function = arguments[3];
            while( changed > 0 ){
                changed = 0;
                for( var i = start_index; i < end_index - 1; i++ ){
                    for( var j = i + 1; j < end_index; j++ ){
                        if( compare_function( array[i], array[j] ) > 0 ){
                            var itemi = array[i];
                            array[i] = array[j];
                            array[j] = itemi;
                            changed++;
                        }
                    }
                }
            }
        }
    }
};

    /*public static Color AliceBlue = new Color( 240, 248, 255 );
    public static Color AntiqueWhite = new Color( 250, 235, 215 );
    public static Color Aqua = new Color( 0, 255, 255 );
    public static Color Aquamarine = new Color( 127, 255, 212 );
    public static Color Azure = new Color( 240, 255, 255 );
    public static Color Beige = new Color( 245, 245, 220 );
    public static Color Bisque = new Color( 255, 228, 196 );
    public static Color Black = new Color( 0, 0, 0 );
    public static Color BlanchedAlmond = new Color( 255, 235, 205 );
    public static Color Blue = new Color( 0, 0, 255 );
    public static Color BlueViolet = new Color( 138, 43, 226 );
    public static Color Brown = new Color( 165, 42, 42 );
    public static Color BurlyWood = new Color( 222, 184, 135 );
    public static Color CadetBlue = new Color( 95, 158, 160 );
    public static Color Chartreuse = new Color( 127, 255, 0 );
    public static Color Chocolate = new Color( 210, 105, 30 );
    public static Color Coral = new Color( 255, 127, 80 );
    public static Color CornflowerBlue = new Color( 100, 149, 237 );
    public static Color Cornsilk = new Color( 255, 248, 220 );
    public static Color Crimson = new Color( 220, 20, 60 );
    public static Color Cyan = new Color( 0, 255, 255 );
    public static Color DarkBlue = new Color( 0, 0, 139 );
    public static Color DarkCyan = new Color( 0, 139, 139 );
    public static Color DarkGoldenrod = new Color( 184, 134, 11 );
    public static Color DarkGray = new Color( 169, 169, 169 );
    public static Color DarkGreen = new Color( 0, 100, 0 );
    public static Color DarkKhaki = new Color( 189, 183, 107 );
    public static Color DarkMagenta = new Color( 139, 0, 139 );
    public static Color DarkOliveGreen = new Color( 85, 107, 47 );
    public static Color DarkOrange = new Color( 255, 140, 0 );
    public static Color DarkOrchid = new Color( 153, 50, 204 );
    public static Color DarkRed = new Color( 139, 0, 0 );
    public static Color DarkSalmon = new Color( 233, 150, 122 );
    public static Color DarkSeaGreen = new Color( 143, 188, 139 );
    public static Color DarkSlateBlue = new Color( 72, 61, 139 );
    public static Color DarkSlateGray = new Color( 47, 79, 79 );
    public static Color DarkTurquoise = new Color( 0, 206, 209 );
    public static Color DarkViolet = new Color( 148, 0, 211 );
    public static Color DeepPink = new Color( 255, 20, 147 );
    public static Color DeepSkyBlue = new Color( 0, 191, 255 );
    public static Color DimGray = new Color( 105, 105, 105 );
    public static Color DodgerBlue = new Color( 30, 144, 255 );
    public static Color Firebrick = new Color( 178, 34, 34 );
    public static Color FloralWhite = new Color( 255, 250, 240 );
    public static Color ForestGreen = new Color( 34, 139, 34 );
    public static Color Fuchsia = new Color( 255, 0, 255 );
    public static Color Gainsboro = new Color( 220, 220, 220 );
    public static Color GhostWhite = new Color( 248, 248, 255 );
    public static Color Gold = new Color( 255, 215, 0 );
    public static Color Goldenrod = new Color( 218, 165, 32 );
    public static Color Gray = new Color( 128, 128, 128 );
    public static Color Green = new Color( 0, 128, 0 );
    public static Color GreenYellow = new Color( 173, 255, 47 );
    public static Color Honeydew = new Color( 240, 255, 240 );
    public static Color HotPink = new Color( 255, 105, 180 );
    public static Color IndianRed = new Color( 205, 92, 92 );
    public static Color Indigo = new Color( 75, 0, 130 );
    public static Color Ivory = new Color( 255, 255, 240 );
    public static Color Khaki = new Color( 240, 230, 140 );
    public static Color Lavender = new Color( 230, 230, 250 );
    public static Color LavenderBlush = new Color( 255, 240, 245 );
    public static Color LawnGreen = new Color( 124, 252, 0 );
    public static Color LemonChiffon = new Color( 255, 250, 205 );
    public static Color LightBlue = new Color( 173, 216, 230 );
    public static Color LightCoral = new Color( 240, 128, 128 );
    public static Color LightCyan = new Color( 224, 255, 255 );
    public static Color LightGoldenrodYellow = new Color( 250, 250, 210 );
    public static Color LightGreen = new Color( 144, 238, 144 );
    public static Color LightGray = new Color( 211, 211, 211 );
    public static Color LightPink = new Color( 255, 182, 193 );
    public static Color LightSalmon = new Color( 255, 160, 122 );
    public static Color LightSeaGreen = new Color( 32, 178, 170 );
    public static Color LightSkyBlue = new Color( 135, 206, 250 );
    public static Color LightSlateGray = new Color( 119, 136, 153 );
    public static Color LightSteelBlue = new Color( 176, 196, 222 );
    public static Color LightYellow = new Color( 255, 255, 224 );
    public static Color Lime = new Color( 0, 255, 0 );
    public static Color LimeGreen = new Color( 50, 205, 50 );
    public static Color Linen = new Color( 250, 240, 230 );
    public static Color Magenta = new Color( 255, 0, 255 );
    public static Color Maroon = new Color( 128, 0, 0 );
    public static Color MediumAquamarine = new Color( 102, 205, 170 );
    public static Color MediumBlue = new Color( 0, 0, 205 );
    public static Color MediumOrchid = new Color( 186, 85, 211 );
    public static Color MediumPurple = new Color( 147, 112, 219 );
    public static Color MediumSeaGreen = new Color( 60, 179, 113 );
    public static Color MediumSlateBlue = new Color( 123, 104, 238 );
    public static Color MediumSpringGreen = new Color( 0, 250, 154 );
    public static Color MediumTurquoise = new Color( 72, 209, 204 );
    public static Color MediumVioletRed = new Color( 199, 21, 133 );
    public static Color MidnightBlue = new Color( 25, 25, 112 );
    public static Color MintCream = new Color( 245, 255, 250 );
    public static Color MistyRose = new Color( 255, 228, 225 );
    public static Color Moccasin = new Color( 255, 228, 181 );
    public static Color NavajoWhite = new Color( 255, 222, 173 );
    public static Color Navy = new Color( 0, 0, 128 );
    public static Color OldLace = new Color( 253, 245, 230 );
    public static Color Olive = new Color( 128, 128, 0 );
    public static Color OliveDrab = new Color( 107, 142, 35 );
    public static Color Orange = new Color( 255, 165, 0 );
    public static Color OrangeRed = new Color( 255, 69, 0 );
    public static Color Orchid = new Color( 218, 112, 214 );
    public static Color PaleGoldenrod = new Color( 238, 232, 170 );
    public static Color PaleGreen = new Color( 152, 251, 152 );
    public static Color PaleTurquoise = new Color( 175, 238, 238 );
    public static Color PaleVioletRed = new Color( 219, 112, 147 );
    public static Color PapayaWhip = new Color( 255, 239, 213 );
    public static Color PeachPuff = new Color( 255, 218, 185 );
    public static Color Peru = new Color( 205, 133, 63 );
    public static Color Pink = new Color( 255, 192, 203 );
    public static Color Plum = new Color( 221, 160, 221 );
    public static Color PowderBlue = new Color( 176, 224, 230 );
    public static Color Purple = new Color( 128, 0, 128 );
    public static Color Red = new Color( 255, 0, 0 );
    public static Color RosyBrown = new Color( 188, 143, 143 );
    public static Color RoyalBlue = new Color( 65, 105, 225 );
    public static Color SaddleBrown = new Color( 139, 69, 19 );
    public static Color Salmon = new Color( 250, 128, 114 );
    public static Color SandyBrown = new Color( 244, 164, 96 );
    public static Color SeaGreen = new Color( 46, 139, 87 );
    public static Color SeaShell = new Color( 255, 245, 238 );
    public static Color Sienna = new Color( 160, 82, 45 );
    public static Color Silver = new Color( 192, 192, 192 );
    public static Color SkyBlue = new Color( 135, 206, 235 );
    public static Color SlateBlue = new Color( 106, 90, 205 );
    public static Color SlateGray = new Color( 112, 128, 144 );
    public static Color Snow = new Color( 255, 250, 250 );
    public static Color SpringGreen = new Color( 0, 255, 127 );
    public static Color SteelBlue = new Color( 70, 130, 180 );
    public static Color Tan = new Color( 210, 180, 140 );
    public static Color Teal = new Color( 0, 128, 128 );
    public static Color Thistle = new Color( 216, 191, 216 );
    public static Color Tomato = new Color( 255, 99, 71 );
    public static Color Turquoise = new Color( 64, 224, 208 );
    public static Color Violet = new Color( 238, 130, 238 );
    public static Color Wheat = new Color( 245, 222, 179 );
    public static Color White = new Color( 255, 255, 255 );
    public static Color WhiteSmoke = new Color( 245, 245, 245 );
    public static Color Yellow = new Color( 255, 255, 0 );
    public static Color YellowGreen = new Color( 154, 205, 50 );
    public static InternalStdOut stdout = new InternalStdOut();
    public static InternalStdErr stderr = new InternalStdErr();*/


org.kbinani.PortUtil.YES_OPTION = 0;//int
org.kbinani.PortUtil.NO_OPTION = 1;//int
org.kbinani.PortUtil.CANCEL_OPTION = 2;//int
org.kbinani.PortUtil.OK_OPTION = 0;//int
org.kbinani.PortUtil.CLOSED_OPTION = -1;//int

org.kbinani.PortUtil.JCT11280=Function('var a="zKV33~jZ4zN=~ji36XazM93y!{~k2y!o~k0ZlW6zN?3Wz3W?{EKzK[33[`y|;-~j^YOTz$!~kNy|L1$353~jV3zKk3~k-4P4zK_2+~jY4y!xYHR~jlz$_~jk4z$e3X5He<0y!wy|X3[:~l|VU[F3VZ056Hy!nz/m1XD61+1XY1E1=1y|bzKiz!H034zKj~mEz#c5ZA3-3X$1~mBz$$3~lyz#,4YN5~mEz#{ZKZ3V%7Y}!J3X-YEX_J(3~mAz =V;kE0/y|F3y!}~m>z/U~mI~j_2+~mA~jp2;~m@~k32;~m>V}2u~mEX#2x~mBy+x2242(~mBy,;2242(~may->2&XkG2;~mIy-_2&NXd2;~mGz,{4<6:.:B*B:XC4>6:.>B*BBXSA+A:X]E&E<~r#z+625z s2+zN=`HXI@YMXIAXZYUM8X4K/:Q!Z&33 3YWX[~mB`{zKt4z (zV/z 3zRw2%Wd39]S11z$PAXH5Xb;ZQWU1ZgWP%3~o@{Dgl#gd}T){Uo{y5_d{e@}C(} WU9|cB{w}bzvV|)[} H|zT}d||0~{]Q|(l{|x{iv{dw}(5}[Z|kuZ }cq{{y|ij}.I{idbof%cu^d}Rj^y|-M{ESYGYfYsZslS`?ZdYO__gLYRZ&fvb4oKfhSf^d<Yeasc1f&a=hnYG{QY{D`Bsa|u,}Dl|_Q{C%xK|Aq}C>|c#ryW=}eY{L+`)][YF_Ub^h4}[X|?r|u_ex}TL@YR]j{SrXgo*|Gv|rK}B#mu{R1}hs|dP{C7|^Qt3|@P{YVV |8&}#D}ef{e/{Rl|>Hni}R1{Z#{D[}CQlQ||E}[s{SG_+i8eplY[=[|ec[$YXn#`hcm}YR|{Ci(_[ql|?8p3]-}^t{wy}4la&pc|3e{Rp{LqiJ],] `kc(]@chYnrM`O^,ZLYhZB]ywyfGY~aex!_Qww{a!|)*lHrM{N+n&YYj~Z b c#e_[hZSon|rOt`}hBXa^i{lh|<0||r{KJ{kni)|x,|0auY{D!^Sce{w;|@S|cA}Xn{C1h${E]Z-XgZ*XPbp]^_qbH^e[`YM|a||+=]!Lc}]vdBc=j-YSZD]YmyYLYKZ9Z>Xcczc2{Yh}9Fc#Z.l{}(D{G{{mRhC|L3b#|xK[Bepj#ut`H[,{E9Yr}1b{[e]{ZFk7[ZYbZ0XL]}Ye[(`d}c!|*y`Dg=b;gR]Hm=hJho}R-[n}9;{N![7k_{UbmN]rf#pTe[x8}!Qcs_rs[m`|>N}^V})7{^r|/E}),}HH{OYe2{Skx)e<_.cj.cjoMhc^d}0uYZd!^J_@g,[[[?{i@][|3S}Yl3|!1|eZ|5IYw|1D}e7|Cv{OHbnx-`wvb[6[4} =g+k:{C:}ed{S]|2M]-}WZ|/q{LF|dYu^}Gs^c{Z=}h>|/i|{W]:|ip{N:|zt|S<{DH[p_tvD{N<[8Axo{X4a.^o^X>Yfa59`#ZBYgY~_t^9`jZHZn`>G[oajZ;X,i)Z.^~YJe ZiZF^{][[#Zt^|]Fjx]&_5dddW]P0C[-]}]d|y {C_jUql] |OpaA[Z{lp|rz}:Mu#]_Yf6{Ep?f5`$[6^D][^u[$[6^.Z8]]ePc2U/=]K^_+^M{q*|9tYuZ,s(dS{i=|bNbB{uG}0jZOa:[-]dYtu3]:]<{DJ_SZIqr_`l=Yt`gkTnXb3d@kiq0a`Z{|!B|}e}Ww{Sp,^Z|0>_Z}36|]A|-t}lt{R6pi|v8hPu#{C>YOZHYmg/Z4nicK[}hF_Bg|YRZ7c|crkzYZY}_iXcZ.|)U|L5{R~qi^Uga@Y[xb}&qdbd6h5|Btw[}c<{Ds53[Y7]?Z<|e0{L[ZK]mXKZ#Z2^tavf0`PE[OSOaP`4gi`qjdYMgys/?[nc,}EEb,eL]g[n{E_b/vcvgb.{kcwi`~v%|0:|iK{Jh_vf5lb}KL|(oi=LrzhhY_^@`zgf[~g)[J_0fk_V{T)}I_{D&_/d9W/|MU[)f$xW}?$xr4<{Lb{y4}&u{XJ|cm{Iu{jQ}CMkD{CX|7A}G~{kt)nB|d5|<-}WJ}@||d@|Iy}Ts|iL|/^|no|0;}L6{Pm]7}$zf:|r2}?C_k{R(}-w|`G{Gy[g]bVje=_0|PT{^Y^yjtT[[[l!Ye_`ZN]@[n_)j3nEgMa]YtYpZy].d-Y_cjb~Y~[nc~sCi3|zg}B0}do{O^{|$`_|D{}U&|0+{J3|8*]iayx{a{xJ_9|,c{Ee]QXlYb]$[%YMc*]w[aafe]aVYi[fZEii[xq2YQZHg]Y~h#|Y:thre^@^|_F^CbTbG_1^qf7{L-`VFx Zr|@EZ;gkZ@slgko`[e}T:{Cu^pddZ_`yav^Ea+[#ZBbSbO`elQfLui}.F|txYcbQ`XehcGe~fc^RlV{D_0ZAej[l&jShxG[ipB_=u:eU}3e8[=j|{D(}dO{Do[BYUZ0/]AYE]ALYhZcYlYP/^-^{Yt_1_-;YT`P4BZG=IOZ&]H[e]YYd[9^F[1YdZxZ?Z{Z<]Ba2[5Yb[0Z4l?]d_;_)a?YGEYiYv`_XmZs4ZjY^Zb]6gqGaX^9Y}dXZr[g|]Y}K aFZp^k^F]M`^{O1Ys]ZCgCv4|E>}8eb7}l`{L5[Z_faQ|c2}Fj}hw^#|Ng|B||w2|Sh{v+[G}aB|MY}A{|8o}X~{E8paZ:]i^Njq]new)`-Z>haounWhN}c#{DfZ|fK]KqGZ=:u|fqoqcv}2ssm}.r{]{nIfV{JW)[K|,Z{Uxc|]l_KdCb%]cfobya3`p}G^|LZiSC]U|(X|kBlVg[kNo({O:g:|-N|qT}9?{MBiL}Sq{`P|3a|u.{Uaq:{_o|^S}jX{Fob0`;|#y_@[V[K|cw[<_ }KU|0F}d3|et{Q7{LuZttsmf^kYZ`Af`}$x}U`|Ww}d]| >}K,r&|XI|*e{C/a-bmr1fId4[;b>tQ_:]hk{b-pMge]gfpo.|(w[jgV{EC1Z,YhaY^q,_G[c_g[J0YX]`[h^hYK^_Yib,` {i6vf@YM^hdOKZZn(jgZ>bzSDc^Z%[[o9[2=/YHZ(_/Gu_`*|8z{DUZxYt^vuvZjhi^lc&gUd4|<UiA`z]$b/Z?l}YI^jaHxe|;F}l${sQ}5g}hA|e4}?o{ih}Uz{C)jPe4]H^J[Eg[|AMZMlc}:,{iz}#*|gc{Iq|/:|zK{l&}#u|myd{{M&v~nV};L|(g|I]ogddb0xsd7^V})$uQ{HzazsgxtsO^l}F>ZB]r|{7{j@cU^{{CbiYoHlng]f+nQ[bkTn/}<-d9q {KXadZYo+n|l[|lc}V2{[a{S4Zam~Za^`{HH{xx_SvF|ak=c^[v^7_rYT`ld@]:_ub%[$[m](Shu}G2{E.ZU_L_R{tz`vj(f?^}hswz}GdZ}{S:h`aD|?W|`dgG|if{a8|J1{N,}-Ao3{H#{mfsP|[ bzn+}_Q{MT{u4kHcj_q`eZj[8o0jy{p7}C|[}l){MuYY{|Ff!Ykn3{rT|m,^R|,R}$~Ykgx{P!]>iXh6[l[/}Jgcg{JYZ.^qYfYIZl[gZ#Xj[Pc7YyZD^+Yt;4;`e8YyZVbQ7YzZxXja.7SYl[s]2^/Ha$[6ZGYrb%XiYdf2]H]kZkZ*ZQ[ZYS^HZXcCc%Z|[(bVZ]]:OJQ_DZCg<[,]%Zaa [g{C00HY[c%[ChyZ,Z_`PbXa+eh`^&jPi0a[ggvhlekL]w{Yp^v}[e{~;k%a&k^|nR_z_Qng}[E}*Wq:{k^{FJZpXRhmh3^p>de^=_7`|ZbaAZtdhZ?n4ZL]u`9ZNc3g%[6b=e.ZVfC[ZZ^^^hD{E(9c(kyZ=bb|Sq{k`|vmr>izlH[u|e`}49}Y%}FT{[z{Rk}Bz{TCc/lMiAqkf(m$hDc;qooi[}^o:c^|Qm}a_{mrZ(pA`,}<2sY| adf_%|}`}Y5U;}/4|D>|$X{jw{C<|F.hK|*A{MRZ8Zsm?imZm_?brYWZrYx`yVZc3a@f?aK^ojEd {bN}/3ZH]/$YZhm^&j 9|(S|b]mF}UI{q&aM]LcrZ5^.|[j`T_V_Gak}9J[ ZCZD|^h{N9{~&[6Zd{}B}2O|cv]K}3s}Uy|l,fihW{EG`j_QOp~Z$F^zexS`dcISfhZBXP|.vn|_HYQ|)9|cr]<`&Z6]m_(ZhPcSg>`Z]5`~1`0Xcb4k1{O!bz|CN_T{LR|a/gFcD|j<{Z._[f)mPc:1`WtIaT1cgYkZOaVZOYFrEe[}T$}Ch}mk{K-^@]fH{Hdi`c*Z&|Kt{if[C{Q;{xYB`dYIX:ZB[}]*[{{p9|4GYRh2ao{DS|V+[zd$`F[ZXKadb*A] Ys]Maif~a/Z2bmclb8{Jro_rz|x9cHojbZ{GzZx_)]:{wAayeDlx}<=`g{H1{l#}9i|)=|lP{Qq}.({La|!Y{i2EZfp=c*}Cc{EDvVB|;g}2t{W4av^Bn=]ri,|y?|3+}T*ckZ*{Ffr5e%|sB{lx^0]eZb]9[SgAjS_D|uHZx]dive[c.YPkcq/}db{EQh&hQ|eg}G!ljil|BO]X{Qr_GkGl~YiYWu=c3eb}29v3|D|}4i||.{Mv})V{SP1{FX}CZW6{cm|vO{pS|e#}A~|1i}81|Mw}es|5[}3w{C`h9aL]o{}p[G`>i%a1Z@`Ln2bD[$_h`}ZOjhdTrH{[j_:k~kv[Sdu]CtL}41{I |[[{]Zp$]XjxjHt_eThoa#h>sSt8|gK|TVi[Y{t=}Bs|b7Zpr%{gt|Yo{CS[/{iteva|cf^hgn}($_c^wmb^Wm+|55jrbF|{9^ q6{C&c+ZKdJkq_xOYqZYSYXYl`8]-cxZAq/b%b*_Vsa[/Ybjac/OaGZ4fza|a)gY{P?| I|Y |,pi1n7}9bm9ad|=d{aV|2@[(}B`d&|Uz}B}{`q|/H|!JkM{FU|CB|.{}Az}#P|lk}K{|2rk7{^8^?`/|k>|Ka{Sq}Gz}io{DxZh[yK_#}9<{TRdgc]`~Z>JYmYJ]|`!ZKZ]gUcx|^E[rZCd`f9oQ[NcD_$ZlZ;Zr}mX|=!|$6ZPZYtIo%fj}CpcN|B,{VDw~gb}@hZg`Q{LcmA[(bo`<|@$|o1|Ss}9Z_}tC|G`{F/|9nd}i=}V-{L8aaeST]daRbujh^xlpq8|}zs4bj[S`J|]?G{P#{rD{]I`OlH{Hm]VYuSYUbRc*6[j`8]pZ[bt_/^Jc*[<Z?YE|Xb|?_Z^Vcas]h{t9|Uwd)_(=0^6Zb{Nc} E[qZAeX[a]P^|_J>e8`W^j_Y}R{{Jp__]Ee#e:iWb9q_wKbujrbR}CY`,{mJ}gz{Q^{t~N|? gSga`V_||:#mi}3t|/I`X{N*|ct|2g{km}gi|{={jC}F;|E}{ZZjYf*frmu}8Tdroi{T[|+~}HG{cJ}DM{Lp{Ctd&}$hi3|FZ| m}Kr|38}^c|m_|Tr{Qv|36}?Up>|;S{DV{k_as}BK{P}}9p|t`jR{sAm4{D=b4pWa[}Xi{EjwEkI}3S|E?u=X0{jf} S|NM|JC{qo^3cm]-|JUx/{Cj{s>{Crt[UXuv|D~|j|d{YXZR}Aq}0r}(_{pJfi_z}0b|-vi)Z mFe,{f4|q`b{}^Z{HM{rbeHZ|^x_o|XM|L%|uFXm}@C_{{Hhp%a7|0p[Xp+^K}9U{bP}: tT}B|}+$|b2|[^|~h{FAby[`{}xgygrt~h1[li`c4vz|,7p~b(|mviN}^pg[{N/|g3|^0c,gE|f%|7N{q[|tc|TKA{LU}I@|AZp(}G-sz{F |qZ{}F|f-}RGn6{Z]_5})B}UJ{FFb2]4ZI@v=k,]t_Dg5Bj]Z-]L]vrpdvdGlk|gF}G]|IW}Y0[G| /bo|Te^,_B}#n^^{QHYI[?hxg{[`]D^IYRYTb&kJ[cri[g_9]Ud~^_]<p@_e_XdNm-^/|5)|h_{J;{kacVopf!q;asqd}n)|.m|bf{QW|U)}b+{tL|w``N|to{t ZO|T]jF}CB|0Q{e5Zw|k |We}5:{HO{tPwf_uajjBfX}-V_C_{{r~gg|Ude;s+}KNXH}! `K}eW{Upwbk%ogaW}9EYN}YY|&v|SL{C3[5s.]Y]I]u{M6{pYZ`^,`ZbCYR[1mNg>rsk0Ym[jrE]RYiZTr*YJ{Ge|%-lf|y(`=[t}E6{k!|3)}Zk} ][G{E~cF{u3U.rJ|a9p#o#ZE|?|{sYc#vv{E=|LC}cu{N8`/`3`9rt[4|He{cq|iSYxY`}V |(Q|t4{C?]k_Vlvk)BZ^r<{CL}#h}R+[<|i=}X|{KAo]|W<`K{NW|Zx}#;|fe{IMr<|K~tJ_x}AyLZ?{GvbLnRgN}X&{H7|x~}Jm{]-| GpNu0}.ok>|c4{PYisrDZ|fwh9|hfo@{H~XSbO]Odv]%`N]b1Y]]|eIZ}_-ZA]aj,>eFn+j[aQ_+]h[J_m_g]%_wf.`%k1e#Z?{CvYu_B^|gk`Xfh^M3`afGZ-Z|[m{L}|k3cp[it ^>YUi~d>{T*}YJ{Q5{Jxa$hg|%4`}|LAgvb }G}{P=|<;Ux{_skR{cV|-*|s-{Mp|XP|$G|_J}c6cM{_=_D|*9^$ec{V;|4S{qO|w_|.7}d0|/D}e}|0G{Dq]Kdp{}dfDi>}B%{Gd|nl}lf{C-{y}|ANZr}#={T~|-(}c&{pI|ft{lsVP}){|@u}!W|bcmB{d?|iW|:dxj{PSkO|Hl]Li:}VYk@|2={fnWt{M3`cZ6|)}|Xj}BYa?vo{e4|L7|B7{L7|1W|lvYO}W8nJ|$Vih|{T{d*_1|:-n2dblk``fT{Ky|-%}m!|Xy|-a{Pz}[l{kFjz|iH}9N{WE{x,|jz}R {P|{D)c=nX|Kq|si}Ge{sh|[X{RF{t`|jsr*fYf,rK|/9}$}}Nf{y!1|<Std}4Wez{W${Fd_/^O[ooqaw_z[L`Nbv[;l7V[ii3_PeM}.h^viqYjZ*j1}+3{bt{DR[;UG}3Og,rS{JO{qw{d<_zbAh<R[1_r`iZTbv^^a}c{iEgQZ<exZFg.^Rb+`Uj{a+{z<[~r!]`[[|rZYR|?F|qppp]L|-d|}K}YZUM|=Y|ktm*}F]{D;g{uI|7kg^}%?Z%ca{N[_<q4xC]i|PqZC]n}.bDrnh0Wq{tr|OMn6tM|!6|T`{O`|>!]ji+]_bTeU}Tq|ds}n|{Gm{z,f)}&s{DPYJ`%{CGd5v4tvb*hUh~bf]z`jajiFqAii]bfy^U{Or|m+{I)cS|.9k:e3`^|xN}@Dnlis`B|Qo{`W|>||kA}Y}{ERYuYx`%[exd`]|OyiHtb}HofUYbFo![5|+]gD{NIZR|Go}.T{rh^4]S|C9_}xO^i`vfQ}C)bK{TL}cQ|79iu}9a];sj{P.o!f[Y]pM``Jda^Wc9ZarteBZClxtM{LW}l9|a.mU}KX}4@{I+f1}37|8u}9c|v${xGlz}jP{Dd1}e:}31}%3X$|22i<v+r@~mf{sN{C67G97855F4YL5}8f{DT|xy{sO{DXB334@55J1)4.G9A#JDYtXTYM4, YQD9;XbXm9SX]IB^4UN=Xn<5(;(F3YW@XkH-X_VM[DYM:5XP!T&Y`6|,^{IS-*D.H>:LXjYQ0I3XhAF:9:(==.F*3F1189K/7163D,:@|e2{LS36D4hq{Lw/84443@4.933:0307::6D7}&l{Mx657;89;,K5678H&93D(H<&<>0B90X^I;}Ag1{P%3A+>><975}[S{PZE453?4|T2{Q+5187;>447:81{C=hL6{Me^:=7ii{R=.=F<81;48?|h8}Uh{SE|,VxL{ST,7?9Y_5Xk3A#:$%YSYdXeKXOD8+TXh7(@>(YdXYHXl9J6X_5IXaL0N?3YK7Xh!1?XgYz9YEXhXaYPXhC3X`-YLY_XfVf[EGXZ5L8BXL9YHX]SYTXjLXdJ: YcXbQXg1PX]Yx4|Jr{Ys4.8YU+XIY`0N,<H%-H;:0@,74/:8546I=9177154870UC]d<C3HXl7ALYzXFXWP<<?E!88E5@03YYXJ?YJ@6YxX-YdXhYG|9o{`iXjY_>YVXe>AYFX[/(I@0841?):-B=14337:8=|14{c&93788|di{cW-0>0<097/A;N{FqYpugAFT%X/Yo3Yn,#=XlCYHYNX[Xk3YN:YRT4?)-YH%A5XlYF3C1=NWyY}>:74-C673<69545v {iT85YED=64=.F4..9878/D4378?48B3:7:7/1VX[f4{D,{l<5E75{dAbRB-8-@+;DBF/$ZfW8S<4YhXA.(5@*11YV8./S95C/0R-A4AXQYI7?68167B95HA1*<M3?1/@;/=54XbYP36}lc{qzSS38:19?,/39193574/66878Yw1X-87E6=;964X`T734:>86>1/=0;(I-1::7ALYGXhF+Xk[@W%TYbX7)KXdYEXi,H-XhYMRXfYK?XgXj.9HX_SX]YL1XmYJ>Y}WwIXiI-3-GXcYyXUYJ$X`Vs[7;XnYEZ;XF! 3;%8;PXX(N3Y[)Xi1YE&/ :;74YQ6X`33C;-(>Xm0(TYF/!YGXg8 9L5P01YPXO-5%C|qd{{/K/E6,=0144:361:955;6443@?B7*7:F89&F35YaX-CYf,XiFYRXE_e{}sF 0*7XRYPYfXa5YXXY8Xf8Y~XmA[9VjYj*#YMXIYOXk,HHX40YxYMXU8OXe;YFXLYuPXP?EB[QV0CXfY{:9XV[FWE0D6X^YVP*$4%OXiYQ(|xp|%c3{}V`1>Y`XH00:8/M6XhQ1:;3414|TE|&o@1*=81G8<3}6<|(f6>>>5-5:8;093B^3U*+*^*UT30XgYU&7*O1953)5@E78--F7YF*B&0:%P68W9Zn5974J9::3}Vk|-,C)=)1AJ4+<3YGXfY[XQXmT1M-XcYTYZXCYZXEYXXMYN,17>XIG*SaS|/eYJXbI?XdNZ+WRYP<F:R PXf;0Xg`$|1GX9YdXjLYxWX!ZIXGYaXNYm6X9YMX?9EXmZ&XZ#XQ>YeXRXfAY[4 ;0X!Zz0XdN$XhYL XIY^XGNXUYS/1YFXhYk.TXn4DXjB{jg|4DEX]:XcZMW=A.+QYL<LKXc[vV$+&PX*Z3XMYIXUQ:ZvW< YSXFZ,XBYeXMM)?Xa XiZ4/EXcP3%}&-|6~:1(-+YT$@XIYRBC<}&,|7aJ6}bp|8)K1|Xg|8C}[T|8Q.89;-964I38361<=/;883651467<7:>?1:.}le|:Z=39;1Y^)?:J=?XfLXbXi=Q0YVYOXaXiLXmJXO5?.SFXiCYW}-;|=u&D-X`N0X^,YzYRXO(QX_YW9`I|>hZ:N&X)DQXP@YH#XmNXi$YWX^=!G6YbYdX>XjY|XlX^XdYkX>YnXUXPYF)FXT[EVTMYmYJXmYSXmNXi#GXmT3X8HOX[ZiXN]IU2>8YdX1YbX<YfWuZ8XSXcZU%0;1XnXkZ_WTG,XZYX5YSX Yp 05G?XcYW(IXg6K/XlYP4XnI @XnO1W4Zp-9C@%QDYX+OYeX9>--YSXkD.YR%Q/Yo YUX].Xi<HYEZ2WdCE6YMXa7F)=,D>-@9/8@5=?7164;35387?N<618=6>7D+C50<6B03J0{Hj|N9$D,9I-,.KB3}m |NzE0::/81YqXjMXl7YG; [.W=Z0X4XQY]:MXiR,XgM?9$9>:?E;YE77VS[Y564760391?14941:0=:8B:;/1DXjFA-564=0B3XlH1+D85:0Q!B#:-6&N/:9<-R3/7Xn<*3J4.H:+334B.=>30H.;3833/76464665755:/83H6633:=;.>5645}&E|Y)?1/YG-,93&N3AE@5 <L1-G/8A0D858/30>8<549=@B8] V0[uVQYlXeD(P#ID&7T&7;Xi0;7T-$YE)E=1:E1GR):--0YI7=E<}n9|aT6783A>D7&4YG7=391W;Zx<5+>F#J39}o/|cc;6=A050EQXg8A1-}D-|d^5548083563695D?-.YOXd37I$@LYLWeYlX<Yd+YR A$;3-4YQ-9XmA0!9/XLY_YT(=5XdDI>YJ5XP1ZAW{9>X_6R(XhYO65&J%DA)C-!B:97#A9;@?F;&;(9=11/=657/H,<8}bz|j^5446>.L+&Y^8Xb6?(CYOXb*YF(8X`FYR(XPYVXmPQ%&DD(XmZXW??YOXZXfCYJ79,O)XnYF7K0!QXmXi4IYFRXS,6<%-:YO(+:-3Q!1E1:W,Zo}Am|n~;3580534*?3Zc4=9334361693:30C<6/717:<1/;>59&:4}6!|rS36=1?75<8}[B|s809983579I.A.>84758=108564741H*9E{L{|u%YQ<%6XfH.YUXe4YL@,>N}Tv|ve*G0X)Z;/)3@A74(4P&A1X:YVH97;,754*A66:1 D739E3553545558E4?-?K17/770843XAYf838A7K%N!YW4.$T19Z`WJ*0XdYJXTYOXNZ 1XaN1A+I&Xi.Xk3Z3GB&5%WhZ1+5#Y[X<4YMXhQYoQXVXbYQ8XSYUX4YXBXWDMG0WxZA[8V+Z8X;D],Va$%YeX?FXfX[XeYf<X:Z[WsYz8X_Y]%XmQ(!7BXIZFX]&YE3F$(1XgYgYE& +[+W!<YMYFXc;+PXCYI9YrWxGXY9DY[!GXiI7::)OC;*$.>N*HA@{C|}&k=:<TB83X`3YL+G4XiK]i}(fYK<=5$.FYE%4*5*H*6XkCYL=*6Xi6!Yi1KXR4YHXbC8Xj,B9ZbWx/XbYON#5B}Ue}+QKXnF1&YV5XmYQ0!*3IXBYb71?1B75XmF;0B976;H/RXU:YZX;BG-NXj;XjI>A#D3B636N;,*%<D:0;YRXY973H5)-4FXOYf0:0;/7759774;7;:/855:543L43<?6=E,.A4:C=L)%4YV!1(YE/4YF+ F3%;S;&JC:%/?YEXJ4GXf/YS-EXEYW,9;E}X$}547EXiK=51-?71C%?57;5>463553Zg90;6447?<>4:9.7538XgN{|!}9K/E&3-:D+YE1)YE/3;37/:05}n<}:UX8Yj4Yt864@JYK..G=.(A Q3%6K>3(P3#AYE$-6H/456*C=.XHY[#S.<780191;057C)=6HXj?955B:K1 E>-B/9,;5.!L?:0>/.@//:;7833YZ56<4:YE=/:7Z_WGC%3I6>XkC*&NA16X=Yz2$X:Y^&J48<99k8}CyB-61<18K946YO4{|N}E)YIB9K0L>4=46<1K0+R;6-=1883:478;4,S+3YJX`GJXh.Yp+Xm6MXcYpX(>7Yo,/:X=Z;Xi0YTYHXjYmXiXj;*;I-8S6N#XgY}.3XfYGO3C/$XjL$*NYX,1 6;YH&<XkK9C#I74.>}Hd`A748X[T450[n75<4439:18A107>|ET}Rf<1;14876/Yb983E<5.YNXd4149>,S=/4E/<306443G/06}0&}UkYSXFYF=44=-5095=88;63844,9E6644{PL}WA8:>)7+>763>>0/B3A545CCnT}Xm|dv}Xq1L/YNXk/H8;;.R63351YY747@15YE4J8;46;.38.>4A369.=-83,;Ye3?:3@YE.4-+N353;/;@(X[YYD>@/05-I*@.:551741Yf5>6A443<3535;.58/86=D4753442$635D1>0359NQ @73:3:>><Xn?;43C14 ?Y|X611YG1&<+,4<*,YLXl<1/AIXjF*N89A4Z576K1XbJ5YF.ZOWN.YGXO/YQ01:4G38Xl1;KI0YFXB=R<7;D/,/4>;$I,YGXm94@O35Yz66695385.>:6A#5}W7n^4336:4157597434433<3|XA}m`>=D>:4A.337370?-6Q96{`E|4A}C`|Qs{Mk|J+~r>|o,wHv>Vw}!c{H!|Gb|*Ca5}J||,U{t+{CN[!M65YXOY_*B,Y[Z9XaX[QYJYLXPYuZ%XcZ8LY[SYPYKZM<LMYG9OYqSQYM~[e{UJXmQYyZM_)>YjN1~[f3{aXFY|Yk:48YdH^NZ0|T){jVFYTZNFY^YTYN~[h{nPYMYn3I]`EYUYsYIZEYJ7Yw)YnXPQYH+Z.ZAZY]^Z1Y`YSZFZyGYHXLYG 8Yd#4~[i|+)YH9D?Y^F~Y7|-eYxZ^WHYdYfZQ~[j|3>~[k|3oYmYqY^XYYO=Z*4[]Z/OYLXhZ1YLZIXgYIHYEYK,<Y`YEXIGZI[3YOYcB4SZ!YHZ*&Y{Xi3~[l|JSY`Zz?Z,~[m|O=Yi>??XnYWXmYS617YVYIHZ(Z4[~L4/=~[n|Yu{P)|];YOHHZ}~[o33|a>~[r|aE]DH~[s|e$Zz~[t|kZFY~XhYXZB[`Y}~[u|{SZ&OYkYQYuZ2Zf8D~[v}% ~[w3},Q[X]+YGYeYPIS~[y}4aZ!YN^!6PZ*~[z}?E~[{3}CnZ=~[}}EdDZz/9A3(3S<,YR8.D=*XgYPYcXN3Z5 4)~[~}JW=$Yu.XX~] }KDX`PXdZ4XfYpTJLY[F5]X~[2Yp}U+DZJ::<446[m@~]#3}]1~]%}^LZwZQ5Z`/OT<Yh^ -~]&}jx[ ~m<z!%2+~ly4VY-~o>}p62yz!%2+Xf2+~ly4VY-zQ`z (=] 2z~o2",C={" ":0,"!":1},c=34,i=2,p,s="",u=String.fromCharCode,t=u(12539);while(++c<127)C[u(c)]=c^39&&c^92?i++:0;i=0;while(0<=(c=C[a.charAt(i++)]))if(16==c)if((c=C[a.charAt(i++)])<87){if(86==c)c=1879;while(c--)s+=u(++p)}else s+=s.substr(8272,360);else if(c<86)s+=u(p+=c<51?c-16:(c-55)*92+C[a.charAt(i++)]);else if((c=((c-86)*92+C[a.charAt(i++)])*92+C[a.charAt(i++)])<49152)s+=u(p=c<40960?c:c|57344);else{c&=511;while(c--)s+=t;p=12539}return s')();

org.kbinani.PortUtil.escapeSJIS=function(str){
    return str.replace(/[^*+.-9A-Z_a-z-]/g,function(s){
        var c=s.charCodeAt(0),m;
        return c<128?(c<16?"%0":"%")+c.toString(16).toUpperCase():65376<c&&c<65440?"%"+(c-65216).toString(16).toUpperCase():(c=org.kbinani.PortUtil.JCT11280.indexOf(s))<0?"%81E":"%"+((m=((c<8272?c:(c=JCT11280.lastIndexOf(s)))-(c%=188))/188)<31?m+129:m+193).toString(16).toUpperCase()+(64<(c+=c<63?64:65)&&c<91||95==c||96<c&&c<123?String.fromCharCode(c):"%"+c.toString(16).toUpperCase())
    })
};

org.kbinani.PortUtil.unescapeUTF8=function(str){
    return str.replace(/%(E(0%[AB]|[1-CEF]%[89AB]|D%[89])[0-9A-F]|C[2-9A-F]|D[0-9A-F])%[89AB][0-9A-F]|%[0-7][0-9A-F]/ig,function(s){
        var c=parseInt(s.substring(1),16);
        return String.fromCharCode(c<128?c:c<224?(c&31)<<6|parseInt(s.substring(4),16)&63:((c&15)<<6|parseInt(s.substring(4),16)&63)<<6|parseInt(s.substring(7),16)&63)
    })
};

/// <summary>
/// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
/// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
/// </summary>
/// <param name="obj">[Object]</param>
/// <returns>[String]</returns>
org.kbinani.PortUtil.getComponentName = function( obj ){
    if( obj == null ){
        return "";
    }
    return obj.getAttribute( "id" );
};

/*public static String formatMessage( String patern, Object... args ){
    return MessageFormat.format( patern, args );
}*/

/// <summary>
/// 単位は秒
/// </summary>
/// <returns>[double]</returns>
org.kbinani.PortUtil.getCurrentTime = function(){
    var d = new Date();
    return d.getTime() / 1000.0;
};

/*public static int getCurrentModifierKey() {
    int ret = 0;
    if( s_ctrl ){
        ret += InputEvent.CTRL_MASK;
    }
    if( s_alt ){
        ret += InputEvent.ALT_MASK;
    }
    if( s_shift ){
        ret += InputEvent.SHIFT_MASK;
    }
    return ret;
}*/

/*public static Rectangle getScreenBounds( Component w ){
    return w.getGraphicsConfiguration().getBounds();
}*/

/*public static void setClipboardText( String value ) {
    Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
    clip.setContents( new StringSelection( value ), null );
}

public static void clearClipboard() {
    Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
    clip.setContents( new StringSelection( null ), null );
}

public static boolean isClipboardContainsText() {
    Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
    Transferable data = clip.getContents( null );

    if( data == null || !data.isDataFlavorSupported( DataFlavor.stringFlavor ) ){
        return true;
    }else{
        return false;
    }
}

public static String getClipboardText() {
    Clipboard clip = Toolkit.getDefaultToolkit().getSystemClipboard();
    Transferable data = clip.getContents( null );

    String str = null;
    if( data == null || !data.isDataFlavorSupported( DataFlavor.stringFlavor ) ){
        str = null;
    }else{
        try {
            str = (String)data.getTransferData( DataFlavor.stringFlavor );
        }catch( Exception e ){
            str = null;
        }
    }
    return str;
}*/

/**
 * @param data [long]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_int64_le = function( data ) {
    var dat = new Array( 8 );
    dat[0] = data & 0xff;
    data = data >>> 8;
    dat[1] = data & 0xff;
    data = data >>> 8;
    dat[2] = data & 0xff;
    data = data >>> 8;
    dat[3] = data & 0xff;
    data = data >>> 8;
    dat[4] = data & 0xff;
    data = data >>> 8;
    dat[5] = data & 0xff;
    data = data >>> 8;
    dat[6] = data & 0xff;
    data = data >>> 8;
    dat[7] = data & 0xff;
    return dat;
},

/**
 * @param data [long]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_uint32_le = function( data ){
    var dat = new Array( 4 );
    data = 0xffffffff & data;
    dat[0] = data & 0xff;
    data = data >>> 8;
    dat[1] = data & 0xff;
    data = data >>> 8;
    dat[2] = data & 0xff;
    data = data >>> 8;
    dat[3] = data & 0xff;
    return dat;
};

/**
 * @param data [int]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_int32_le = function( data ){
    var v = data;
    if( v < 0 ){
        v += 4294967296;
    }
    return getbytes_uint32_le( v );
};

/**
 * @param data [int]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_int32_be = function( data ){
    var v = data;
    if( v < 0 ){
        v += 4294967296;
    }
    return getbytes_uint32_be( v );
};

/**
 * @param data [long]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_int64_be = function( data ){
    var dat = new Array( 8 );
    dat[7] = data & 0xff;
    data = data >>> 8;
    dat[6] = data & 0xff;
    data = data >>> 8;
    dat[5] = data & 0xff;
    data = data >>> 8;
    dat[4] = data & 0xff;
    data = data >>> 8;
    dat[3] = data & 0xff;
    data = data >>> 8;
    dat[2] = data & 0xff;
    data = data >>> 8;
    dat[1] = data & 0xff;
    data = data >>> 8;
    dat[0] = data & 0xff;
    return dat;
};

/**
 * @param data [long]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_uint32_be = function( data ) {
    var dat = new Array( 4 );
    data = 0xffffffff & data;
    dat[3] = data & 0xff;
    data = data >>> 8;
    dat[2] = data & 0xff;
    data = data >>> 8;
    dat[1] = data & 0xff;
    data = data >>> 8;
    dat[0] = data & 0xff;
    return dat;
};

/**
 * @param data [short]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_int16_le = function( data ) {
    var i = data;
    if ( i < 0 ) {
        i += 65536;
    }
    return getbytes_uint16_le( i );
};

/// <summary>
/// compatible to BitConverter
/// </summary>
/// <param name="buf"></param>
/// <returns></returns>
/**
 * @param data [int]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_uint16_le = function( data ) {
    var dat = new Array( 2 );
    dat[0] = data & 0xff;
    data = data >>> 8;
    dat[1] = data & 0xff;
    return dat;
};

/**
 * @param data [int]
 * @return [byte[]]
 */
org.kbinani.PortUtil.getbytes_uint16_be = function( data ) {
    var dat = new Array( 2 );
    dat[1] = data & 0xff;
    data = data >>> 8;
    dat[0] = data & 0xff;
    return dat;
};

/**
 * @param buf [byte[]]
 * @return [long]
 */
org.kbinani.PortUtil.make_int64_le = function( buf ) {
    return ((((((((((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
};

/**
 * @param buf [byte[]]
 * @return [long]
 */
org.kbinani.PortUtil.make_int64_be = function( buf ) {
    return ((((((((((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
};

/**
 * @param buf [byte[]]
 * @return [long]
 */
org.kbinani.PortUtil.make_uint32_le = function() {
    var buf = arguments[0];
    var index = 0;
    if( arguments.length >= 2 ){
        index = arguments[1];
    }
    return ((((((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
};

/**
 * @param buf [byte[]]
 * @return [long]
 */
org.kbinani.PortUtil.make_uint32_be = function() {
    var buf = arguments[0];
    var index = 0;
    if( arguments.length >= 2 ){
        index = arguments[1];
    }
    return ((((((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
};

/**
 * @param buf [byte[]]
 * @return [int]
 */
org.kbinani.PortUtil.make_int32_le = function( buf ) {
    var v = make_uint32_le( buf );
    if ( v >= 2147483647 ) {
        v -= 4294967296;
    }
    return v;
};

/**
 * @param buf [byte[]]
 * @param index [int]
 * @return [int]
 */
org.kbinani.PortUtil.make_uint16_le = function( buf, index ) {
    return (((0xff & buf[index + 1]) << 8) | (0xff & buf[index]));
};

/**
 * @param buf [byte[]]
 * @return [int]
 */
org.kbinani.PortUtil.make_uint16_le = function( buf ) {
    return make_uint16_le( buf, 0 );
};

/**
 * @param buf [byte[]]
 * @return [int]
 */
org.kbinani.PortUtil.make_uint16_be = function() {
    var buf = arguments[0];
    var index = 0;
    if( arguments.length >= 2 ){
        index = arguments[1];
    }
    return (((0xff & buf[index]) << 8) | (0xff & buf[index + 1]));
};

/**
 * @param buf [byte[]]
 * @param index [int]
 * @return [short]
 */
org.kbinani.PortUtil.make_int16_le = function( buf, index ) {
    var i = make_uint16_le( buf, index );
    if ( i >= 32768 ) {
        i = i - 65536;
    }
    return i;
};

/**
 * @param buf [byte[]]
 * @return [short]
 */
org.kbinani.PortUtil.make_int16_le = function( buf ) {
    return make_int16_le( buf, 0 );
};

/*public static double make_double_le( byte[] buf ) {
    long n = 0L;
    for ( int i = 7; i >= 0; i-- ) {
        n = (n << 8) | (buf[i] & 0xffL);
    }
    return Double.longBitsToDouble( n );
}*/

/*public static double make_double_be( byte[] buf ) {
    long n = 0L;
    for ( int i = 0; i <= 7; i++ ) {
        n = (n << 8) | (buf[i] & 0xffL);
    }
    return Double.longBitsToDouble( n );
}*/

/*public static float make_float_le( byte[] buf ) {
    int n = 0;
    for ( int i = 3; i >= 0; i-- ) {
        n = (n << 8) | (buf[i] & 0xff);
    }
    return Float.intBitsToFloat( n );
}*/

/*public static float make_float_be( byte[] buf ) {
    int n = 0;
    for ( int i = 0; i <= 3; i++ ) {
        n = (n << 8) | (buf[i] & 0xff);
    }
    return Float.intBitsToFloat( n );
}*/

/*public static byte[] getbytes_double_le( double value ) {
    long n = Double.doubleToLongBits( value );
    return getbytes_int64_le( n );
}*/

/*public static byte[] getbytes_double_be( double value ) {
    long n = Double.doubleToLongBits( value );
    return getbytes_int64_be( n );
}*/

/*public static byte[] getbytes_float_le( float value ) {
    int n = Float.floatToIntBits( value );
    return getbytes_int32_le( n );
}*/

/*public static byte[] getbytes_float_be( float value ) {
    int n = Float.floatToIntBits( value );
    return getbytes_int32_be( n );
}*/

/*public static void drawBezier( Graphics2D g, float x1, float y1,
                   float ctrlx1, float ctrly1,
                   float ctrlx2, float ctrly2,
                   float x2, float y2 ) {
#if JAVA
    g.draw( new CubicCurve2D.Float( x1, y1, ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2 ) );
#else
    Stroke stroke = g.getStroke();
    System.Drawing.Pen pen = null;
    if ( stroke is BasicStroke ) {
        pen = ((BasicStroke)stroke).pen;
    } else {
        pen = new System.Drawing.Pen( System.Drawing.Color.Black );
    }
    g.nativeGraphics.DrawBezier( pen, new System.Drawing.PointF( x1, y1 ),
                                      new System.Drawing.PointF( ctrlx1, ctrly1 ),
                                      new System.Drawing.PointF( ctrlx2, ctrly2 ),
                                      new System.Drawing.PointF( x2, y2 ) );
#endif
}

public const int STRING_ALIGN_FAR = 1;
public const int STRING_ALIGN_NEAR = -1;
public const int STRING_ALIGN_CENTER = 0;
public static void drawStringEx( Graphics g1, String s, Font font, Rectangle rect, int align, int valign ) {
#if JAVA
    Graphics2D g = (Graphics2D)g1;
    g.setFont( font );
    FontMetrics fm = g.getFontMetrics();
    Dimension ret = new Dimension( fm.stringWidth( s ), fm.getHeight() );
    float x = 0;
    float y = 0;
    if( align > 0 ){
        x = rect.x + rect.width - ret.width;
    }else if( align < 0 ){
        x = rect.x;
    }else{
        x = rect.x + rect.width / 2.0f - ret.width / 2.0f;
    }
    if( valign > 0 ){
        y = rect.y + rect.height - ret.height;
    }else if( valign < 0 ){
        y = rect.y;
    }else{
        y = rect.y + rect.height / 2.0f - ret.height / 2.0f;
    }
    g.drawString( s, x, y );
#else
    System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
    if ( align > 0 ) {
        sf.Alignment = System.Drawing.StringAlignment.Far;
    } else if ( align < 0 ) {
        sf.Alignment = System.Drawing.StringAlignment.Near;
    } else {
        sf.Alignment = System.Drawing.StringAlignment.Center;
    }
    if ( valign > 0 ) {
        sf.LineAlignment = System.Drawing.StringAlignment.Far;
    } else if ( valign < 0 ) {
        sf.LineAlignment = System.Drawing.StringAlignment.Near;
    } else {
        sf.LineAlignment = System.Drawing.StringAlignment.Center;
    }
    g1.nativeGraphics.DrawString( s, font.font, g1.brush, new System.Drawing.RectangleF( rect.x, rect.y, rect.width, rect.height ), sf );
#endif
}
#endregion

#region System.IO
public static double getFileLastModified( String path ) {
#if JAVA
    File f = new File( path );
    if( f.exists() ){
        return f.lastModified();
    }else{
        return 0.0;
    }
#else
    if ( File.Exists( path ) ) {
        return new FileInfo( path ).LastWriteTimeUtc.Ticks * 100.0 / 1e9;
    }
    return 0;
#endif
}

public static long getFileLength( String fpath ) {
#if JAVA
    File f = new File( fpath );
    return f.length();
#else
    return new FileInfo( fpath ).Length;
#endif
}

public static String getExtension( String fpath ) {
#if JAVA
    String name = (new File( fpath )).getName();
    int index = name.lastIndexOf( '.' );
    if( index <= 0 ){
        return "";
    }else{
        return name.substring( index );
    }
#else
    return Path.GetExtension( fpath );
#endif
}

public static String getFileName( String path ) {
#if JAVA
    File f = new File( path );
    return f.getName();
#else
    return Path.GetFileName( path );
#endif
}

public static String getDirectoryName( String path ) {
#if JAVA
    File f = new File( path );
    return f.getParent();
#else
    return System.IO.Path.GetDirectoryName( path );
#endif
}

public static String getFileNameWithoutExtension( String path ) {
#if JAVA
    String file = getFileName( path );
    int index = file.lastIndexOf( file );
    if( index > 0 ){
        file = file.substring( 0, index );
    }
    return file;
#else
    return System.IO.Path.GetFileNameWithoutExtension( path );
#endif
}

public static String createTempFile() {
#if JAVA
    String ret = "";
    try{
        File.createTempFile( "tmp", "" ).getAbsolutePath();
    }catch( Exception ex ){
        System.out.println( "PortUtil#createTempFile; ex=" + ex );
    }
    return ret;
#else
    return System.IO.Path.GetTempFileName();
#endif
}

public static String[] listFiles( String directory, String extension ) {
#if JAVA
    File f = new File( directory );
    File[] list = f.listFiles();
    Vector<String> ret = new Vector<String>();
    for( int i = 0; i < list.length; i++ ){
        File t = list[i];
        if( !t.isDirectory() ){
            String name = t.getName();
            if( name.endsWith( extension ) ){
                ret.add( name );
            }
        }
    }
    return ret.toArray( new String[]{} );
#else
    return System.IO.Directory.GetFiles( directory, "*" + extension );
#endif
}

public static void deleteFile( String path ) {
#if JAVA
    new File( path ).delete();
#else
    System.IO.File.Delete( path );
#endif
}

public static void moveFile( String pathBefore, String pathAfter ) 
#if JAVA
    throws java.io.FileNotFoundException, java.io.IOException
#endif
{
#if JAVA
    copyFile( pathBefore, pathAfter );
    deleteFile( pathBefore );
#else
    System.IO.File.Move( pathBefore, pathAfter );
#endif
}

public static boolean isDirectoryExists( String path ) {
#if JAVA
    File f = new File( path );
    if( f.exists() ){
        if( f.isFile() ){
            return true;
        }else{
            return false;
        }
    }else{
        return false;
    }
#else
    return Directory.Exists( path );
#endif
}

public static boolean isFileExists( String path ) {
#if JAVA
    return (new File( path )).exists();
#else
    return System.IO.File.Exists( path );
#endif
}

public static String combinePath( String path1, String path2 ) {
#if JAVA
    if( path1 != null && path1.endsWith( File.separator ) ){
        path1 = path1.substring( 0, path1.length() - 1 );
    }
    if( path2 != null && path2.startsWith( File.separator ) ){
        path2 = path2.substring( 1 );
    }
    return path1 + File.separator + path2;
#else
    return System.IO.Path.Combine( path1, path2 );
#endif
}

public static String getTempPath() {
#if JAVA
    String ret = System.getProperty( "java.io.tmpdir" );
    if( ret == null ){
        return "";
    }else{
        return ret;
    }
#else
    return Path.GetTempPath();
#endif
}

public static void createDirectory( String path ) {
#if JAVA
    File f = new File( path );
    f.mkdir();
#else
    Directory.CreateDirectory( path );
#endif
}

public static void deleteDirectory( String path, boolean recurse ) {
#if JAVA
    File f = new File( path );
    File[] list = f.listFiles();
    for( int i = 0; i < list.length; i++ ){
        File f0 = new File( combinePath( path, list[i].getName() ) );
        if( f0.isDirectory() ){
            deleteDirectory( f0.getPath(), true );
        }else{
            f0.delete();
        }
    }
#else
    Directory.Delete( path, recurse );
#endif
}

public static void deleteDirectory( String path ) {
#if JAVA
    (new File( path )).delete();
#else
    Directory.Delete( path );
#endif
}

public static void copyFile( String file1, String file2 )
#if JAVA
    throws FileNotFoundException, IOException
#endif
{
#if JAVA
    FileChannel sourceChannel = new FileInputStream( new File( file1 ) ).getChannel();
    FileChannel destinationChannel = new FileOutputStream( new File( file2 ) ).getChannel();
    sourceChannel.transferTo( 0, sourceChannel.size(), destinationChannel );
    sourceChannel.close();
    destinationChannel.close();
#else
    File.Copy( file1, file2 );
#endif
}
#endregion*/

/**
 * @param s [string]
 * @param value [ByRef<int>]
 * @return [bool]
 */
org.kbinani.PortUtil.tryParseInt = function( s, value ) {
    var v = parseInt( s, 10 );
    if( isNaN( v ) ){
        value.value = 0;
        return false;
    }else{
        value.value = v;
        return true;
    }
};

/*public static boolean tryParseFloat( String s, ByRef<Float> value ) {
    try {
        value.value = parseFloat( s );
    } catch ( Exception ex ) {
        return false;
    }
    return true;
}

public static int parseInt( String value ) {
#if JAVA
    return Integer.parseInt( value );
#else
    return int.Parse( value );
#endif
}

public static float parseFloat( String value ) {
#if JAVA
    return Float.parseFloat( value );
#else
    return float.Parse( value );
#endif
}

public static double parseDouble( String value ) {
#if JAVA
    return Double.parseDouble( value );
#else
    return double.Parse( value );
#endif
}

public static String formatDecimal( String format, double value ) {
#if JAVA
    DecimalFormat df = new DecimalFormat( format );
    return df.format( value );
#else
    return value.ToString( format );
#endif
}

public static String formatDecimal( String format, long value ) {
#if JAVA
    DecimalFormat df = new DecimalFormat( format );
    return df.format( value );
#else
    return value.ToString( format );
#endif
}

public static String toHexString( long value, int digits ) {
    String ret = toHexString( value );
    int add = digits - getStringLength( ret );
    for ( int i = 0; i < add; i++ ) {
        ret = "0" + ret;
    }
    return ret;
}

public static String toHexString( long value ) {
#if JAVA
    return Long.toHexString( value );
#else
    return Convert.ToString( value, 16 );
#endif
}

public static long fromHexString( String s ) {
#if JAVA
    return Long.parseLong( s, 16 );
#else
    return Convert.ToInt64( s, 16 );
#endif
}
#endregion*/

org.kbinani.PortUtil.splitString = function(){
    if( typeof( arguments[1] ) == "object" ){
        // splitString( String s, String[] separator, int count, bool ignore_empty_entries );
        // たぶんarguments[1]の型はstringのArrayだろう。
        var s = arguments[0];
        var separator = arguments[1]; //String[]
        var count = -1;
        if( arguments.length >= 3 ){
            count = arguments[2];
        }
        var ignore_empty_entries = false;
        if( arguments.length >= 4 ){
            ignore_empty_entries = arguments[3];
        }
        var ret = new Array();
        if( separator.length == 0 ){
            ret.push( s );
            return ret;
        }
        var remain = s;
        var len = separator.length;
        var index = remain.indexOf( separator[0] );
        var i = 1;
        while( index < 0 && i < separator.length ){
            index = remain.indexOf( separator[i] );
        }
        var added_count = 0;
        while( index >= 0 ){
            if( !ignore_empty_entries || (ignore_empty_entries && index > 0) ){
                if( added_count + 1 == count ){
                    break;
                }else{
                    ret.push( remain.substring( 0, index ) );
                }
                added_count++;
            }
            remain = remain.substring( index + len );
            index = remain.indexOf( separator[0] );
            i = 1;
            while( index < 0 && i < separator.length ){
                index = remain.indexOf( separator[i] );
            }
        }
        if( !ignore_empty_entries || (ignore_empty_entries && remain.length() > 0) ){
            ret.push( remain );
        }
        return ret;
    }else if( typeof( arguments[1] ) == "string" ){
        // splitString( String s, String separator );
        var s = arguments[0];
        var separator = arguments[1];
        return s.split( separator );
    }
};

/*public static int getStringLength( String s ) {
    if ( s == null ) {
        return 0;
    } else {
#if JAVA
        return s.length();
#else
        return s.Length;
#endif
    }
}

public static int getEncodedByteCount( String encoding, String str ) {
    byte[] buf = getEncodedByte( encoding, str );
#if JAVA
    return buf.length;
#else
    return buf.Length;
#endif
}

public static byte[] getEncodedByte( String encoding, String str ) {
#if JAVA
    Charset enc = Charset.forName( encoding );
    ByteBuffer bb = enc.encode( str );
    byte[] dat = new byte[bb.limit()];
    bb.get( dat );
    return dat;
#else
    Encoding enc = Encoding.GetEncoding( encoding );
    return enc.GetBytes( str );
#endif
}*/

/**
 * @param encoding [String]
 * @param data [byte[]]
 * @param offset [int]
 * @param length [int]
 * @return [String]
 */
/*public static String getDecodedString( String encoding, byte[] data, int offset, int length ) {
    Charset enc = Charset.forName( encoding );
    ByteBuffer bb = ByteBuffer.allocate( length );
    bb.put( data, offset, length );
    return enc.decode( bb ).toString();
};*/

/*public static String getDecodedString( String encoding, byte[] data ) {
#if JAVA
    return getDecodedString( encoding, data, 0, data.length );
#else
    return getDecodedString( encoding, data, 0, data.Length );
#endif
}

#endregion

public static void setMousePosition( Point p ) {
#if JAVA
    // TODO: PortUtil#setMousePosition
#else
    System.Windows.Forms.Cursor.Position = new System.Drawing.Point( p.x, p.y );
#endif
}

public static Point getMousePosition() {
#if JAVA
    return MouseInfo.getPointerInfo().getLocation();
#else
    System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
    return new Point( p.X, p.Y );
#endif
}

/// <summary>
/// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
/// </summary>
/// <param name="p"></param>
/// <returns></returns>
public static boolean isPointInScreens( Point p ) {
#if JAVA
    GraphicsEnvironment ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
    GraphicsDevice[] gs = ge.getScreenDevices();
    for (int j = 0; j < gs.length; j++) { 
        GraphicsDevice gd = gs[j];
        Rectangle rc = gd.getDefaultConfiguration().getBounds();
        if( rc.x <= p.x && p.x <= rc.x + rc.width ){
            if( rc.y <= p.y && p.y <= rc.y + rc.height ){
                return true;
            }
        }
    }
    return false;
#else
    foreach ( System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens ) {
        System.Drawing.Rectangle rc = screen.WorkingArea;
        if ( rc.X <= p.x && p.x <= rc.X + rc.Width ) {
            if ( rc.Y <= p.y && p.y <= rc.Y + rc.Height ) {
                return true;
            }
        }
    }
    return false;
#endif
}

#if JAVA
public static Rectangle getWorkingArea( Window w ){
    return w.getGraphicsConfiguration().getBounds();
#else
public static Rectangle getWorkingArea( System.Windows.Forms.Form w ) {
    System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea( w );
    return new Rectangle( r.X, r.Y, r.Width, r.Height );
#endif
}

public static String getMD5FromString( String str ) {
#if JAVA
    MessageDigest digest = null;
    try {
        digest = MessageDigest.getInstance("MD5");
        byte[] buff = getEncodedByte( "UTF-8", str );
        digest.update( buff, 0, buff.length );
    } catch( NoSuchAlgorithmException ex2 ){
        System.err.println( "PortUtil#getMD5FromString; ex2=" + ex2 );
    }
    byte[] dat = digest.digest();
    String ret = "";
    for( int i = 0; i < dat.length; i++ ){
        ret += String.format( "%02x", dat[i] );
    }
    return ret;
#else
    return Misc.getmd5( str );
#endif
}

public static String getMD5( String file )
#if JAVA
    throws FileNotFoundException, IOException
#endif
{
#if JAVA
    InputStream in = new FileInputStream( file );
    MessageDigest digest = null;
    try {
        digest = MessageDigest.getInstance("MD5");
        byte[] buff = new byte[4096];
        int len = 0;
        while ((len = in.read(buff, 0, buff.length)) >= 0) {
            digest.update(buff, 0, len);
        }
    } catch (IOException e) {
        throw e;
    } catch( NoSuchAlgorithmException ex2 ){
        System.out.println( "PortUtil#getMD5; ex2=" + ex2 );
    } finally {
        if (in != null) {
            try {
                in.close();
            } catch (IOException e) {
            }
        }
    }
    byte[] dat = digest.digest();
    String ret = "";
    for( int i = 0; i < dat.length; i++ ){
        ret += String.format( "%02x", dat[i] );
    }
    return ret;
#else
    String ret = "";
    using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
        ret = Misc.getmd5( fs );
    }
    return ret;
#endif
}


#if JAVA
class FileFilterImp implements FileFilter{
    private String m_extension;

    public FileFilterImp( String extension ){
        m_extension = extension;
    }

    public boolean accept( File f ){
        String file = f.getName();
        if( file.endsWith( m_extension ) ){
            return true;
        }else{
            return false;
        }
    }
}
#endif

#region Array conversion
public static Integer[] convertIntArray( int[] arr ) {
#if JAVA
    Integer[] ret = new Integer[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
#else
    return arr;
#endif
}

public static Long[] convertLongArray( long[] arr ) {
#if JAVA
    Long[] ret = new Long[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
#else
    return arr;
#endif
}

public static Byte[] convertByteArray( byte[] arr ) {
#if JAVA
    Byte[] ret = new Byte[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
#else
    return arr;
#endif
}

public static Float[] convertFloatArray( float[] arr ) {
#if JAVA
    Float[] ret = new Float[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
#else
    return arr;
#endif
}

public static Character[] convertCharArray( char[] arr ) {
#if JAVA
    Character[] ret = new Character[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
#else
    return arr;
#endif
}

#if JAVA
public static int[] convertIntArray( Integer[] arr ){
    int[] ret = new int[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
}

public static long[] convertLongArray( Long[] arr ){
    long[] ret = new long[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
}

public static byte[] convertByteArray( Byte[] arr ){
    byte[] ret = new byte[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
}

public static float[] convertFloatArray( Float[] arr ){
    float[] ret = new float[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
}

public static char[] convertFloatArray( Character[] arr ){
    char[] ret = new char[arr.length];
    for( int i = 0; i < arr.length; i++ ){
        ret[i] = arr[i];
    }
    return ret;
}
#endif
#endregion

public static String getApplicationStartupPath() {
#if JAVA
    return System.getProperty( "user.dir" );
#else
    return System.Windows.Forms.Application.StartupPath;
#endif
}

public static void println( String s ) {
#if JAVA
    System.out.println( s );
#else
    Console.WriteLine( s );
#endif
}*/
