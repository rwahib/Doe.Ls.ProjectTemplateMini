define(['jquery'], function ($) {

    return {
        isJQueryObject: function (element) {

            return (element instanceof jQuery);

        },

        hasValue: function (val) {
            return hasValue(val);

        },

        randomizeList: function (list) {
            var arr = [];
            for (var k = 0; k < list.length; k++) {
                arr.push(list[k]);
            }

            for (var j, x, i = arr.length; i; j = parseInt(Math.random() * i), x = arr[--i], arr[i] = arr[j], arr[j] = x);
            return arr;
        },

        copyArray: function (array) {
            var tmp = [];
            $(array).each(function () {
                tmp.push(this);


            });
            return tmp;
        },

        urlMatched: function (url) {

            var value = /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url);

            return value;
        },

        isValidExceptionError: function (jqxhr, exc) {
            //for all validation errors
            if (jqxhr.status === 0) {
                return false;
            }
            return true;

        },

        sortByName: function (a, b) {
            var aId = a.id;
            var bId = b.id;
            return ((aId < bId) ? -1 : ((aId > bId) ? 1 : 0));
        },

        disableLink: function (link) {
            $(link).attr('disabled', 'disabled');
            $(link).unbind("click");
        },

        enableLink: function (link, clickHandler) {

            $(link).removeAttr('disabled');
            if (hasValue(clickHandler)) {//if (clickHandler != null && clickHandler != 'undefined') {
                $(link).unbind('click');
                $(link).click(clickHandler);
            }


        },

        GUID: function () {
            var s4 = function () {
                return Math.floor(
                    Math.random() * 0x10000 /* 65536 */
                ).toString(16);
            };
            return (
                s4() + s4() + "-" +
                    s4() + "-" +
                    s4() + "-" +
                    s4() + "-" +
                    s4() + s4() + s4()
            );
        },

        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]").toLowerCase();
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.search.toLowerCase());
            if (results === null)
                return null;
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        },


        getToken: function (value) {
            return value.replace(' ', '-').replace(' ', '-').replace('//', '-').replace('/', '-').replace('\\', '-').replace(':', '-');
        },

        wordfy: function (val) {

            return replaceAll(val, '-', ' ', true);
        },

        replaceAll: function (str, token, newToken, ignoreCase) {
            var i = -1, tokenTemp = '';
            if (typeof token === "string") {
                tokenTemp = ignoreCase === true ? token.toLowerCase() : undefined;
                while ((i = (
                    tokenTemp !== undefined ?
                        str.toLowerCase().indexOf(
                            tokenTemp,
                            i >= 0 ? i + newToken.length : 0
                        ) : str.indexOf(
                            token,
                            i >= 0 ? i + newToken.length : 0
                        )
                )) !== -1) {
                    str = str.substring(0, i)
                        .concat(newToken)
                        .concat(str.substring(i + token.length));
                }
            }
            return str;
        },

        isValidEmail: function (sEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) {
                return true;
            } else {
                return false;
            }
        },

        isValidNumber: function (value, greaterThan) {
            return this.getFloatValue(value) > greaterThan;
        },

        getFloatValue: function (value) {
            var floatValue = parseFloat(value);
            if (isNaN(floatValue))
                floatValue = 0;

            return floatValue;
        },

        isValidTime: function (sTime) {
            var filter = /^\d{1,}:\d{2}$/;

            if (filter.test(sTime)) {
                return true;
            }

            return false;
        },

        convertTimeToFloat: function (sTime) {
            var timeFormat = sTime.match(/([\d]*):([\d]+)/);

            if (this.hasValue(timeFormat)) {
                var hours = parseInt(timeFormat[1]);
                var minutes = parseFloat(timeFormat[2] / 60);
                return this.getFloatValue(hours + minutes);
            } else {
                return 0;
            }
        },

        convertFloatToTime: function (fValue) {
            var secs = fValue * 60 * 60;
            var hours = Math.floor(secs / 3600);
            var minutes = Math.ceil((secs - (hours * 3600)) / 60);

            return hours + ':' + ('00' + minutes).slice(-2);
        },

        isDateInFuture: function (strDate) {
            var tokenised = strDate.split('-');
            var dateValue = new Date(tokenised[0] + '-' + tokenised[1] + '-' + tokenised[2]);
            var today = new Date();

            return (dateDiff(dateValue, today) > 0);
        },

        isDateInPast: function (strDate, limit) {
            var tokenised = strDate.split('-');
            var dateValue = new Date(tokenised[0] + '-' + tokenised[1] + '-' + tokenised[2]);
            var today = new Date();

            var lastDate = new Date();
            lastDate.setFullYear(today.getFullYear() - limit);

            return (dateDiff(dateValue, lastDate) < 0);
        },
        isAlpahNumeric: function (s) {
            var pattern = /^[a-z0-9]+$/i;
            if (s.match(pattern)) {
                return true;
            }
            return false;
        },

        getSimpleDateFromJsonDate:function(jsonDate) {
            if (!hasValue(jsonDate)) return jsonDate;
            var date = new Date(new Date(parseInt(jsonDate.substr(6))));
            if (isNaN(date.getDate())) {
                return null;
            }
            var result = formatNumber(date.getDate()) + '/' + (date.getMonth()+1) + '/' + date.getFullYear();
            return result;
        },

        getDateTimeFromJsonDate: function (jsonDate) {
            if (!hasValue(jsonDate)) return jsonDate;
            var date = new Date(new Date(parseInt(jsonDate.substr(6))));
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            var result = formatNumber(date.getDate()) + '-' + months[date.getMonth()] + '-' + date.getFullYear() + ' ' + formatNumber(date.getHours()) + ':' + formatNumber(date.getMinutes()) + ':' + formatNumber(date.getSeconds());
            return result;
        },

        getDateFromJsonDate: function (jsonDate) {
            if (!hasValue(jsonDate)) return jsonDate;
            var date = new Date(new Date(parseInt(jsonDate.substr(6))));
            if (isNaN(date.getDate())) {
                return null;
            }
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            var result = formatNumber(date.getDate()) + '-' + months[date.getMonth()] + '-' + date.getFullYear();
            return result;
        },

        parselongDate: function (dateString, seperator) {
            if (!this.hasValue(dateString)) {
                throw new Error("Invalid date format");
            }
            if (!this.hasValue(seperator)) {
                seperator = '-';
            }
            var splitDate = dateString.split(seperator);
            var day = parseInt(splitDate[0]);
            var month = parseInt(getMonth(splitDate[1]));
            var year = parseInt(splitDate[2]);

            return new Date(year, month, day);
        },

        getEasyDateFormat: function (jDate) {
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            return formatNumber(jDate.getDate()) + '-' + months[jDate.getMonth()] + '-' + jDate.getFullYear();
        },

        getShortDateFormat: function (jDate) {
            return formatNumber(jDate.getDate()) + '/' + (jDate.getMonth() + 1) + '/' + jDate.getFullYear();
        },

        getLongDateFormat: function(jDate) {
            return getDayOfWeek(jDate) + ', ' + jDate.getDate() + ' ' + getFullMonth(jDate) + ' ' + jDate.getFullYear();
        },

        validDateObject: function (date) {
            return date instanceof Date;
        },

        
        getDayOfWeek: function (date) {
            getDayOfWeek(date);
        },

        getFullMonth: function(date) {
            getFullMonth(date);
        },

        getToday: function () {
            var date = new Date();
            var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            return formatNumber(date.getDate()) + '-' + months[date.getMonth()] + '-' + date.getFullYear();
        },

        getUpdatedSuccessMessage: function (operation, entityFriendlyName) {

            if (operation === "Delete") return 'This ' + entityFriendlyName + ' has been deleted';
            else if (operation === "Edit") return 'This ' + entityFriendlyName + ' has been updated';
            else if (operation === "Create") return 'This ' + entityFriendlyName + ' has been created';

            return "Data has been successfully updated";
        },

        addDays: function (date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        },

        dateDiff: function(date1, date2) {
            return dateDiff(date1, date2);
        },
        
        setSelectEmpty: function (elem, txt) {
            if (!hasValue(txt)) {
                txt = "All";
            }

            elem.empty();
                elem.append($('<option/>', {
                    value: 0,
                    text: txt
                }));
                elem.select2("val", 0);            
        },
       
        containTerms:function(val, terms) {
            return containTerms(val, terms);
        }
    };

    function getFullMonth(date) {
        var months = [
                'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October',
                'November', 'December'
        ];

        return months[date.getMonth()];
    }

    function getDayOfWeek(date) {
        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";

        var day = weekday[date.getDay()];

        return day;
    }

    function dateDiff(date1, date2) {
        var diff = date1.getTime() - date2.getTime();
        return (diff / (24 * 60 * 60 * 1000));
    }

    function formatNumber(number) {
        return ('0' + number).slice(-2);
    }

    function getMonth(monthString) {
        if (!hasValue(monthString)) return -1;

        if (monthString.toLowerCase().indexOf('jan') > -1) return 0;
        if (monthString.toLowerCase().indexOf('feb') > -1) return 1;
        if (monthString.toLowerCase().indexOf('mar') > -1) return 2;

        if (monthString.toLowerCase().indexOf('apr') > -1) return 3;
        if (monthString.toLowerCase().indexOf('may') > -1) return 4;
        if (monthString.toLowerCase().indexOf('jun') > -1) return 5;

        if (monthString.toLowerCase().indexOf('jul') > -1) return 6;
        if (monthString.toLowerCase().indexOf('aug') > -1) return 7;
        if (monthString.toLowerCase().indexOf('sep') > -1) return 8;

        if (monthString.toLowerCase().indexOf('oct') > -1) return 9;
        if (monthString.toLowerCase().indexOf('nov') > -1) return 10;
        if (monthString.toLowerCase().indexOf('dec') > -1) return 11;

        var month = parseInt(monthString);
        if (hasValue(month)) {
            return month - 1;
        }

        return -1;

    }

    function hasValue(val) {
        if (typeof val !== 'undefined' && val !== null) {
            return true;
        }

        return false;
    }

    function containTerms(val, term) {
        val = val.replace(/ +(?= )/g, '');
        term = term.replace(/ +(?= )/g, '');

        val = val.trim().toLowerCase();
        term = term.trim().toLowerCase();
        return val.indexOf(term.trim().toLowerCase())>-1;
        
    }

});