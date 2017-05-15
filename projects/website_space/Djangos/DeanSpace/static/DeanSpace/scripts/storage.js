// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=  Cookie Start -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        var c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name + 1;
            var c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) {
                c_end = document.cookie.length;
                return unescape(document.cookie.substring(c_start, c_end))
            }
        }
    }
    return null;
}

function setCookie(c_name, c_value, expiredays) {
    var exp = new Date()
    exp.setTime(exp.getTime() + expiredays * 24 * 60 * 60 * 1000);
    document.cookie = c_name + "=" + escape(c_value) + "; expires=" + exp.toUTCString();
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=  Cookie End -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=