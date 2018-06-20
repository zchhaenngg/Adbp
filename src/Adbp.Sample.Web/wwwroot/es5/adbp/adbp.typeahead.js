"use strict";

var abp = abp || {};
(function ($) {
	if (!$) {
		throw "dependency jQuery!";
	}
	if (!$.fn.typeahead) {
		throw "require typeahead.js!";
	}

	var libs = {
		defaultOptions: {
			highlight: true,
			minLength: 0
		},
		defaultDataset: {
			limit: 8
		}
	};

	abp.typeahead = function () {
		/*
   * suggestions,  所有数据
   */
		function Typeahead(selector, suggestions) {
			this._selector = selector;
			this._suggestions = suggestions;
			if (!suggestions instanceof Array) {
				throw "suggestions should be array!";
			}
		}

		Typeahead.prototype.getMatches = function (value) {
			if (value.length < 1) {
				//没有输入任何信息时,显示下拉框
				return this._suggestions;
			}
			var matches = [];
			$.each(this._suggestions, function (i, str) {
				if (new RegExp(value, 'i').test(str)) {
					matches.push(str);
				}
			});
			return matches;
		};

		/*
   * match,  value 为用户输入数据
   */
		Typeahead.prototype.search = function (value, sync, async) {
			var matches = this.getMatches(value);
			sync(matches);
		};

		Typeahead.prototype.initialise = function (options, dataset) {
			var _this = this;
			var arg1 = $.extend({}, libs.defaultOptions, options || {});
			var arg2 = $.extend({
				source: function source(value, sync, async) {
					_this.search(value, sync, async);
				}
			}, libs.defaultDataset, dataset || {});
			$(this._selector).typeahead(arg1, arg2);
		};

		return Typeahead;
	}();
})(jQuery);

$(function () {
	$(".adbp-typeahead").each(function () {
		var suggestions = $(this).data("suggestions").split(",");
		new abp.typeahead(this, suggestions).initialise();
	});
});