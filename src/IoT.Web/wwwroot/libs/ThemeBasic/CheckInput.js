
function insertError(e, type) {
    switch (type) {
        case "input":
            $(e).parents(".form-group").addClass("has-error");
            break;
        case "commonbox":
            $(e).parent().addClass("has-error");
        case "remove":
            $(e).parent().removeClass("has-error");
        case "removeSelect":
            $(e).parent().parent().removeClass("has-error");
        case "removeAll":
            var lst = $(e).find(".has-error");
            $(lst).each(function () {
                $(this).removeClass("has-error");
            });
            break;
        case "other":
            $(e).parent().addClass("has-error");
            break;
    }
}

function validateText(str, type, startlength, endlength) {
    var check = true;
    if (typeof str == 'undefined' || !str || str.length === 0 || str.trim() === "" || !/[^\s]/.test(str) || /^\s*$/.test(str) || str.replace(/\s/g, "") === "") {
        return false;
    }
    if (startlength > 0) {
        if (type == "tax") {
            if (str.length == startlength || str.length == endlength) { }
            else {
                return false;
            }
        } else {
            if (str.length < startlength || str.length > endlength)
                return false;
        }
    }
    switch (type) {
        case "email":
            check = validateEmail(str);
            break;
        case "link":
            check = validateLink(str);
            if (check) check = isLinkDate();
            break;
        case "phone":
            check = isNumberPhone(str);
            break;
        case "tax":
            check = isTaxCode(str);
            if (check) check = isTaxCodeDate(str);
            break;
        case "number":
            check = isNumberPhone(str);
            break;
        case "float":
            check = isNumberFloat(str);
            break;
        case "url":
            check = isUrl(str);
            break;
    }
    return check;
}

function validateTextCase(str, type, startlength, endlength) {
    var check = 1;
    if (typeof str == 'undefined' || !str || str.length === 0 || str.trim() === "" || !/[^\s]/.test(str) || /^\s*$/.test(str) || str.replace(/\s/g, "") === "") {
        return 0;
    }
    if (startlength > 0) {
        if (type == "tax") {
            if (str.length == startlength || str.length == endlength) { }
            else {
                return 0;
            }
        } else {
            if (str.length < startlength || str.length > endlength)
                return 0;
        }
    }
    switch (type) {
        case "email":
            check = (validateEmail(str)) ? 1 : 2;
            break;
        case "link":
            check = (validateLink(str)) ? 1 : 2;
            if (check == 1) check = (isLinkDate()) ? 1 : 3;
            break;
        case "phone":
            check = (isNumberPhone(str)) ? 1 : 2;
            break;
        case "tax":
            check = (isTaxCode(str)) ? 1 : 0;
            if (check == 1) check = (isTaxCodeDate(str)) ? 1 : 2;
            break;
        case "linkOut":
            check = (str.indexOf("http") >= 0) ? 1 : 0;
            break;

    }
    return check;
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function isNumberPhone(phone) {
    var re = /^[0-9]+$/g;
    return re.test(phone);
}
function isNumberFloat(float) {
    var re = /^[0-9]+.+[0-9]$/g;
    var re1 = /^[0-9]+$/g;
    let check = re.test(float);
    check = (check) ? check : re1.test(float);
    return check;
}
function isUrl(url) {
    var re = /\b(https?|http):\D+/g;
    return re.test(url);
}

function CoventPrice(price) {
    let stringPrice = "";
    let priceOld = price;
    let i = 0;
    while (price >= 1000 && i < 4) {
        price = price / 1000;
        i = i + 1;
    }
    switch (i) {
        case 2:
            stringPrice = price + " triệu";
            break;
        case 3:
            stringPrice = price + " tỷ";
            break;
        case 4:
            stringPrice = price + " nghìn tỷ";
            break;
        default:
            stringPrice = munberToMoney(priceOld.toString(), true) + " VNĐ";
            break;
    }
    return stringPrice;
}

function munberToMoney(price, check) {
    let re = "";
    if (check) {
        re = price.replace(/,/g, "");
        re = re.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }
    else {
        re = price.replace(/,/g, "");
    }
    return re;
}

function hasUnicode(str) {
    for (var i = 0; i < str.length; i++) {
        if (str.charCodeAt(i) > 127) {
            return true;
        }
    }
    return false;
}

function isUpperCase(str) {
    for (var i = 0; i < str.length; i++) {
        var character = str.charAt(i);
        if (isNaN(character * 1)) {  //character is text
            if (character == character.toLowerCase()) {
                return false;
            }
        }
    }
    return true;
}
function showLabelError(element, text) {
    $(element).parents(".form-group").append("<lable class='lable-error' style='color:#a94442'>" + text + "</lable>");
}