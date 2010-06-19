/**_______________________________________
 *
 *    bytefx :: simple effects in few bytes
 * ---------------------------------------
 *
 * @author              Andrea Giammarchi
 * @site                www.devpro.it
 * @version             0.2
 * @requires		anything *
 * 			* old browsers (IE <= 5) should require JSL
 * @credits		Matteo Galli (aka Ratatuia) for Safari debug,
 * 			Boyan Djumakov, for debug and reports (http://webnos.blogspot.com/)
 * ---------------------------------------
 * 
 * Copyright (c) 2006 Andrea Giammarchi - www.devpro.it
 *
 * Permission is hereby granted, free of charge,
 * to any person obtaining a copy of this software and associated
 * documentation files (the "Software"),
 * to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * _______________________________________
 */
bytefx = {
	alpha:function(element, opacity){
		var	style = bytefx.$element(element).style;
		style.opacity = style.MozOpacity = style.KhtmlOpacity = opacity/100;
		style.filter = "alpha(opacity=" + opacity + ")"
	},
	clear:function(element){
		var	interval = ["size", "move", "fade", "color"],
			i = interval.length;
		while(i--)
			clearInterval(bytefx.$element(element).bytefx[interval[i]]);
	},
	color:function(element, style, start, end, speed, callback){
		end = bytefx.$color2rgb(end);
		clearInterval(bytefx.$element(element).bytefx.color);
		element.bytefx.color = setInterval(function(){
			var	color = bytefx.$color2rgb(start),
				i = 3;
			while(i--)
				color[i] = bytefx.$end(color[i], end[i], speed);
			element.style[style] = start = bytefx.$rgb2color(color);
			if("" + color == "" + end)
				bytefx.$callback(callback, element, "color");
		},1);
	},
	drag:function(element, start, end){
		function $drag(evt){
			var	x = evt.clientX,
				y = evt.clientY;
			if(tmp.start)
				bytefx.position(element, x - tmp.x, y - tmp.y);
			else{
				tmp.x = x - element.offsetLeft;
				tmp.y = y - element.offsetTop;
			}
		};
		function $start(evt){
			tmp.start = true;
			tmp[m.d] = document[m.d];
			tmp[m.u] = document[m.u];
			document[m.u] = element[m.u];
			document[m.d] = function(){return false};
			if(start)
				start.call(element, evt);
			return false;
		};
		function $end(evt){
			tmp.start = false;
			document[m.d] = tmp[m.d];
			document[m.u] = tmp[m.u];
			if(end)
				end.call(element, evt);
			return false;
		};
		var	tmp = bytefx.$element(element).bytefx.drag,
			m = {d:"onmousedown", u:"onmouseup", m:"onmousemove"};
		bytefx.$event(element, m.d, $start);
		bytefx.$event(element, m.u, $end);
		bytefx.$event(document, m.m, $drag);
	},
	fade:function(element, start, end, speed, callback){
		clearInterval(bytefx.$element(element).bytefx.fade);
		element.bytefx.fade = setInterval(function(){
			start = bytefx.$end(start, end, speed);
			bytefx.alpha(element, start);
			if(start == end)
				bytefx.$callback(callback, element, "fade");
		},1);
	},
	move:function(element, position, speed, callback){
		var	start = bytefx.$position(bytefx.$element(element));
		bytefx.$setInterval(element, "move", speed/100, start.x, start.y, position.x, position.y, "position", callback);
	},
	position:function(element, x, y){
		var	style = bytefx.$element(element).style;
		style.position = "absolute";
		style.left = x + "px";
		style.top = y + "px";
	},
	size:function(element, size, speed, callback){
		var	start = bytefx.$size(bytefx.$element(element)),
			tmp = window.opera;
		if(!/msie/i.test(navigator.userAgent) || (tmp && parseInt(tmp.version())>=9)){
			if(size.sw)
				start[0] -= size.sw;
			if(size.sh)
				start[1] -= size.sh;
			if(size.ew)
				size.w -= size.ew;
			if(size.eh)
				size.h -= size.eh;
		};
		element.style.overflow = "hidden";
		bytefx.$setInterval(element, "size", speed/100, start[0], start[1], size.w, size.h, "$size2", callback);
	},
	$callback:function(callback, element, interval){
		clearInterval(element.bytefx[interval]);
		if(callback)
			callback.call(element);
	},
	$color:function(color){
		return bytefx.$rgb2color(bytefx.$color2rgb(color));
	},
	$color2rgb:function(color){
		function c(n){
			return color.charAt(n)
		};
		color = color.substr(1, color.length);
		if(color.length == 3)
			color = c(0) + c(0) + c(1) + c(1) + c(2) + c(2);
		return [parseInt(c(0) + c(1), 16), parseInt(c(2) + c(3), 16), parseInt(c(4) + c(5), 16)];
	},
	$end:function(x, y, speed){
		return x < y ? Math.min(x + speed, y) : Math.max(x - speed, y);
	},
	$element:function(element){
		if(!element.bytefx)
			element.bytefx = {color:0, drag:{}, fade:0, move:0, size:0};
		return element;
	},
	$event:function(element,tmp,callback){
		element[tmp] = (function(value){
			return function(evt){
				if(!evt)
					evt=window.event;
				if(value)
					value.call(this, evt);
				return callback.call(this, evt);
			}
		})(element[tmp]);
	},
	$position:function(element){
		var	position = {x:element.offsetLeft, y:element.offsetTop};
		while(element = element.offsetParent){
			position.x += element.offsetLeft;
			position.y += element.offsetTop;
		};
		return position;
	},
	$rgb2color:function(color){
		function c(n){
			var	tmp = color[n].toString(16);
			return tmp.length == 1 ? "0" + tmp : tmp
		};
		return "#" + c(0) + c(1) + c(2);
	},
	$setInterval:function(element, interval, speed, x, y, w, h, tmp, callback){
		var	round = Math.round;
		clearInterval(element.bytefx[interval]);
		element.bytefx[interval] = setInterval(function(){
			x += (w - x) * speed;
			y += (h - y) * speed;
			bytefx[tmp](element, x, y);
			if(round(x) == w && round(y) == h){
				bytefx[tmp](element, w, h);
				bytefx.$callback(callback, element, interval);
			}
		}, 1);
	},
	$size:function(element){
		var	n = "number",
			size = [0,0];
		if(typeof(element.offsetWidth) == n)
			size = [element.offsetWidth, element.offsetHeight];
		else if(typeof(element.clientWidth) == n)
			size = [element.clientWidth, element.clientHeight];
		else if(typeof(element.innerWidth)==n)
			size = [element.innerWidth, element.innerHeight];
		return size;
	},
	$size2:function(element, w, h){
		var	style = element.style;
		style.width = w + "px";
		style.height = h + "px";
	}
};