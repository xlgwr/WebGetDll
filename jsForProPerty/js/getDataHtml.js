function dataDisplay(dataresult, msgid) {
    if (msgid && dataresult) {

        console.log(msgid);
        $(msgid).html('');
        $(msgid).append('<h2>土 地 登 記 冊 LAND REGISTER</h2>');
        $(msgid).append('<p> <strong>SEARCH DATE AND TIME:</strong>' + dataresult.SEARCH_DATE + '</p>');
        $(msgid).append('<p> <strong>查冊種類 SEARCH TYPE:</strong>' + dataresult.SEARCHType + '</p>');
        $(msgid).append('<p> <strong>本登記冊列明有關物業截至 </strong>' + dataresult.INFORMATIONDate + ' <strong>之資料</strong></p>');
        $(msgid).append('<h2>物業資料 PROPERTY PARTICULARS</h2>');
        $(msgid).append('<p> <strong>PROPERTY REFERENCE NUMBER (PRN):</strong>' + dataresult.PRN + '</p>');
        $(msgid).append('<p> <strong>LOT NO.:</strong>' + dataresult.LotNo + '</p>');
        $(msgid).append('<p><strong>SHARE OF THE LOT:</strong>' + dataresult.ShareOfLot + '</p>');
        $(msgid).append('<p><strong>Address:</strong> ' + dataresult.Address + '</p>');
        $(msgid).append('<p><strong>地址:</strong> ' + dataresult.AddressZH + '</p>');
        $(msgid).append('<hr>');
        $(msgid).append('<h2>業主資料 OWNER PARTICULARS</h2>');
        dataresult.OwnerName.forEach(function (o) {
            $(msgid).append('<p><strong>NAME OF OWNER:</strong>' + o.name + ',<strong>Type:</strong>' + o.type +
                ',<strong>CAPACITY:</strong>' + o.CAPACITY +
                ',<strong>MEMORIAL NO.</strong>' + o.MEMORIALNO +
                ',<strong>DATE OF INSTRUMENT:</strong>' + o.INSTRUMENTDate +
                ',<strong>DATE OF REGISTRATION:</strong>' + o.REGISTRATIONDate +
                ',<strong>CONSIDERATION:</strong>' + o.CONSIDERATION +
                '</p>');
        }, this);
        $(msgid).append('<hr>');
        $(msgid).append('<h2>物業涉及的 INCUMBRANCES</h2>');
        dataresult.InFavourName.forEach(function (o) {
            $(msgid).append('<p><strong>IN FAVOUR OF:</strong>' + o.name + ',<strong>Type:</strong>' + o.type +
                ',<strong>MEMORIAL NO.</strong>' + o.MEMORIALNO +
                ',<strong>DATE OF INSTRUMENT:</strong>' + o.INSTRUMENTDate +
                ',<strong>DATE OF REGISTRATION:</strong>' + o.REGISTRATIONDate +
                ',<strong>NATURE:</strong>' + o.NATURE +
                ',<strong>CONSIDERATION:</strong>' + o.CONSIDERATION +
                '</p>');
        }, this);
    } else {
        if (!dataresult) {
            alert('没有记录');
        } else {
            alert('没有找到标签：' + msgid);
        }

    }
}

function getData(fileData, msgid) {

    //data 
    var dataresult = {
        SEARCH_DATE: undefined,
        SEARCHType: undefined,
        INFORMATIONDate: undefined,
        PRN: undefined,
        LotNo: undefined,
        ShareOfLot: undefined,
        Address: undefined,
        AddressZH: undefined,
        OwnerName: [],
        InFavourName: []
    }

    if (!fileData) {
        console.log("Error:提供的文件内容为空。。。")
        return;
    }
    console.log(fileData.length);

    var $body = $('<div></div>').html(fileData);
    var $Alltable = $body.children('table');

    console.log($Alltable.length);


    //土 地 登 記 冊 LAND REGISTER
    var $talbe0 = $Alltable.eq(0);
    //物業資料 PROPERTY PARTICULARS
    var $talbe1 = $Alltable.eq(1);
    //業主資料 OWNER PARTICULARS
    var $talbe2 = $Alltable.eq(2);
    //物業涉及的 INCUMBRANCES
    var $talbe3 = $Alltable.eq(3);

    for (var t = 0; t < $Alltable.length; t++) {
        var table = $Alltable.eq(t);
        var tableText = table.text();
        if (chkFileType(/LAND\ REGISTER/ig, tableText) && chkFileType(/本登記冊列明有關物業截至/ig, tableText)) {
            $talbe0 = table;
            continue;
        }
        if (chkFileType(/PROPERTY\ PARTICULARS/ig, tableText) && chkFileType(/PROPERTY\ REFERENCE\ NUMBER/ig, tableText)) {
            $talbe1 = table;
            continue;
        }
        if (chkFileType(/OWNER\ PARTICULARS/ig, tableText) && chkFileType(/NAME\ OF\ OWNER/ig, tableText)) {
            $talbe2 = table;
            continue;
        }
        if (chkFileType(/INCUMBRANCES/ig, tableText) && chkFileType(/IN\ FAVOUR\ OF/ig, tableText)) {
            $talbe3 = table;
            continue;
        }
    }


    //土 地 登 記 冊 LAND REGISTER
    var $talbe0Tr = $talbe0.find('tr');
    for (var x = 0; x < $talbe0Tr.length; x++) {
        var tr0 = $talbe0Tr.eq(x);
        tr0.find('br').replaceWith("@");
        var tr0Text = tr0.text().trim();
        if (!tr0Text) continue;

        if (chkFileType(/SEARCH\ TYPE\:/ig, tr0Text) || chkFileType(/SEARCH\ DATE\ AND\ TIME\:/ig)) {
            var tr0TextStr = tr0Text.split('@');
            //console.log(tr0TextStr)
            tr0TextStr.forEach(function (str) {
                if (chkFileType(/SEARCH\ DATE\ AND\ TIME\:/ig, str)) {
                    dataresult.SEARCH_DATE = str.split('TIME:')[1].trim();
                }
                if (chkFileType(/SEARCH\ TYPE\:/ig, str)) {
                    dataresult.SEARCHType = str.split('TYPE:')[1].trim();
                }
            }, this);
            continue;
        }

        if (chkFileType(/本登記冊列明有關物業截至/ig, tr0Text)) {
            //console.log(tr0Text)
            var tr0TextStr = tr0Text.split('@');
            tr0TextStr.forEach(function (str) {
                if (chkFileType(/本登記冊列明有關物業截至/ig, str)) {
                    var dd = str.split(' ');
                    //console.log(dd)
                    dataresult.INFORMATIONDate = dd[1];
                }
            }, this);
            continue;
        }

    }

    //物業資料 PROPERTY PARTICULARS
    var $talbe1Tr = $talbe1.find('tr');
    for (var x = 0; x < $talbe1Tr.length; x++) {
        var tr1 = $talbe1Tr.eq(x);
        var tr1Text = tr1.text().trim();
        if (!tr1Text) continue;

        //console.log(tr1Text);

        if (chkFileType(/PROPERTY\ REFERENCE\ NUMBER\ \(PRN\)\:/ig, tr1Text)) {
            dataresult.PRN = tr1Text.split(':')[1].trim();
            continue;
        }
        if (chkFileType(/lot\ no\.\:/ig, tr1Text)) {

            dataresult.LotNo = tr1.children('td').eq(1).text().trim();
            continue;
        }
        if (chkFileType(/SHARE\ OF\ THE\ LOT\:/ig, tr1Text)) {
            dataresult.ShareOfLot = tr1Text.split(':')[1].trim();
            continue;
        }
        if (chkFileType(/address\:/ig, tr1Text)) {
            var tmptd = tr1.children('td').eq(1);
            var tmptdZh = tr1.children('td').eq(3);
            tmptd.find('br').replaceWith(" ,");
            dataresult.Address = tmptd.text().trim();
            dataresult.AddressZH = tmptdZh.text().trim();
            continue;
        }
    }

    //業主資料 OWNER PARTICULARS
    var $talbe2Tr = $talbe2.find('tr');
    for (var x = 3; x < $talbe2Tr.length; x++) {
        var tr2 = $talbe2Tr.eq(x);
        var _alltd2 = tr2.children('td');
        if (_alltd2.length < 6) continue;
        var tmp_owner = _alltd2.eq(0).text().trim();
        var tmp_CAPACITY = _alltd2.eq(1).text().trim();
        var tmp_MEMORIALNO = _alltd2.eq(2).text().trim();
        var tmp_INSTRUMENTDate = _alltd2.eq(3).text().trim();
        var tmp_REGISTRATIONDate = _alltd2.eq(4).text().trim();
        var tmp_CONSIDERATION = _alltd2.eq(5).text().trim();

        if (chkFileType(/limited|Ltd\.|Co\.|公司/ig, tmp_owner)) {
            //公司

            dataresult.OwnerName.push({
                name: tmp_owner,
                type: '公司',
                CAPACITY: tmp_CAPACITY,
                MEMORIALNO: tmp_MEMORIALNO,
                INSTRUMENTDate: tmp_INSTRUMENTDate,
                REGISTRATIONDate: tmp_REGISTRATIONDate,
                CONSIDERATION: tmp_CONSIDERATION
            });
        } else {
            //个人
            dataresult.OwnerName.push({
                name: tmp_owner,
                type: '个人',
                CAPACITY: tmp_CAPACITY,
                MEMORIALNO: tmp_MEMORIALNO,
                INSTRUMENTDate: tmp_INSTRUMENTDate,
                REGISTRATIONDate: tmp_REGISTRATIONDate,
                CONSIDERATION: tmp_CONSIDERATION
            });
        }
    }

    //物業涉及的 INCUMBRANCES
    var $talbe3Tr = $talbe3.find('tr');
    for (var x = 3; x < $talbe3Tr.length; x++) {
        var tr3 = $talbe3Tr.eq(x);
        var _alltd3 = tr3.children('td');
        if (_alltd3.length < 6) continue;
        var tmp_MEMORIALNO = _alltd3.eq(0).text().trim();
        var tmp_INSTRUMENTDate = _alltd3.eq(1).text().trim();
        var tmp_REGISTRATIONDate = _alltd3.eq(2).text().trim();
        var tmp_NATURE = _alltd3.eq(3).text().trim();
        var tmp_InFavourName = _alltd3.eq(4).text().trim();
        var tmp_CONSIDERATION = _alltd3.eq(5).text().trim();

        if (tmp_InFavourName === '--') continue;

        if (chkFileType(/limited|Ltd\.|Co\.|公司/ig, tmp_InFavourName)) {
            //公司
            dataresult.InFavourName.push({
                name: tmp_InFavourName,
                type: '公司',
                NATURE: tmp_NATURE,
                MEMORIALNO: tmp_MEMORIALNO,
                INSTRUMENTDate: tmp_INSTRUMENTDate,
                REGISTRATIONDate: tmp_REGISTRATIONDate,
                CONSIDERATION: tmp_CONSIDERATION

            });
        } else {
            //个人
            dataresult.InFavourName.push({
                name: tmp_InFavourName,
                type: '个人',
                NATURE: tmp_NATURE,
                MEMORIALNO: tmp_MEMORIALNO,
                INSTRUMENTDate: tmp_INSTRUMENTDate,
                REGISTRATIONDate: tmp_REGISTRATIONDate,
                CONSIDERATION: tmp_CONSIDERATION
            });
        }
    }

    //console.log(dataresult);
    dataDisplay(dataresult, msgid);
}

function getDataHtmlClick(input, msgid) {
    try {
        console.log(input.value);
        if (!input.value) return null;
        if (!chkFileType(/\.htm[l]?$/i, input.value)) {
            console.log("请选择html|htm文件.");
            alert("请选择html|htm文件.");
            input.focus();
            return null;
        }

        //支持chrome IE10
        if (window.FileReader) {
            var file = input.files[0];
            filename = file.name.split(".")[0];
            var reader = new FileReader();
            reader.onload = function () {
                //console.log(this.result);
                getData(this.result, msgid);
                return this.result;
            }
            reader.readAsText(file);
        }
        //支持IE 7 8 9 10
        else if (typeof window.ActiveXObject != 'undefined') {
            var xmlDoc;
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.async = false;
            xmlDoc.load(input.value);
            //console.log(xmlDoc.xml);
            getData(xmlDoc.xml, msgid);

            return xmlDoc.xml;
        }
        //支持FF
        else if (document.implementation && document.implementation.createDocument) {
            var xmlDoc;
            xmlDoc = document.implementation.createDocument("", "", null);
            xmlDoc.async = false;
            xmlDoc.load(input.value);
            //console.log(xmlDoc.xml);
            getData(xmlDoc.xml, msgid);

            return xmlDoc.xml;
        } else {
            alert('error');
            return null;
        }
    } catch (error) {
        alert(error);
        return null;
    }
}

function chkFileType(reg, value) {
    try {
        return reg.test(value);
    } catch (error) {
        alert(error);
        return false;
    }
}