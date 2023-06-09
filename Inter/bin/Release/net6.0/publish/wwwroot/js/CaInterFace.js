var SignStatetimer;
//#region  2.3.2 同步医师结果查询接口
function DocRequestSelect() {
    var openId = openid;
    console.log(openId);
    $.ajax({
        type: "POST",
        url: "/QRCode/DocRequestSelect",
        dataType: "json",
        async: false,
        data: { msspId: openId },
        success: function (data) {
            console.log(data);
            BackData(data.data);
        },
    });
}
//#endregion

//#region   3.3.1 OAuth认证请求接口

function Auth_Request() {
    $.ajax({
        type: "post",
        url: "/QRCode/Auth_Request",
        async: false,
        success: function (d) {
            var data = JSON.parse(d);
            console.log(data.data);
            // 在指定元素中生成二维码
            var qrcode = new QRCode(document.getElementById("qrcode"), {
                text: data.data.qrCode,
                width: 180,
                height: 180,
            });

            $("#qrcode").removeAttr("title");
            requestId = data.data.signDataId;
            timer = setInterval(function () {
                var countdownEl = document.getElementById("countdown"); // 获取数据元素
                var icount = parseInt(countdownEl.textContent); // 获取数据并转换为整数
                icount--; // 每秒减少一个
                countdownEl.textContent = icount; // 更新数据元素内容
                if (icount <= 0) { // 如果数据为零
                    clearInterval(timer); // 停止定时器
                    clearInterval(Backtimer); // 停止定时器
                }
            }, 1000);
        },
    });
}

//#endregion

//#region 3.3.2 OAuth登陆-获取用户信息

function GetUser() {
    $.ajax({
        type: "post",
        url: "/QRCode/GetUser",
        async: false,
        data: { requestId: requestId },
        success: function (d) {
            var data = JSON.parse(d);
            console.log(data);
            console.log(new Date, "第" + i++ + "次执行结果，" + data.message);
            if (data.data.msspId != null && data.data.msspId != undefined && data.data.msspId != "" && data.data.jobStatus == "FINISH") {
                layer.msg('登录成功');
                openid = data.data.msspId;
                CertVail(data.data.signCert)
                signDataId = data.data.signDataId;
                clearTimeout(timer);//清理定时任务
                clearTimeout(Backtimer);//清理定时任务
            }
            else {
                Backtimer = setTimeout(() => { GetUser() }, 2000);
            }
        },
    });
}

//#endregion

//#region  4.3.2 自动签名授权-获取授权结果接口
function Acq_Auth_Res() {
    $.ajax({
        type: "POST",
        url: "/QRCode/Acq_Auth_Res",
        dataType: "json",
        async: false,
        data: { openid: openid },
        success: function (data) {
            clearTimeout(SignStatetimer);
            if (data.Data.grantStep == "1") {
                $("#Display").hide();
                if (type == "Data") {
                    console.log("这是自动签名返回的数据")
                    Data_Signature();
                    Auto_Get_Signature();
                }
                else if (type == "PDF") {
                    console.log("这是自动签名返回的数据")
                    PDF_Signature();
                    Auto_Get_Signature();
                }
                else if (type == "EMR") {
                    EMR_Signature();
                    Auto_Get_Signature();
                }
            }
            else if (data.Data.grantStep == "2") {
                $("#Display").show();
                $("#sign").show();
                Auto_Sign_Auth();
                setTimeout(() => { Acq_Auth_Res() }, 2000);
            }
            else {
                console.log(new Date, "第" + i++ + "次执行结果，" + data.Data.grantStep);
                SignStatetimer = setTimeout(() => { Acq_Auth_Res() }, 2000);
            }
        },
    });
}

//#endregion

//#region 4.3.3 自动签名授权 - 退出授权接口
function LogOut_Auth() {
    var openid = openid;
    $.ajax({
        type: "POST",
        url: "/QRCode/LogOut_Auth",
        dataType: "json",
        async: false,
        data: { openid: openid },
        success: function (data) {
            console.log(data);
        },
    });
}
//#endregion

//#region 4.3.4 数据哈希计算接口

function Data_Hash() {
    var originData = originData
    $.ajax({
        type: "POST",
        url: "/QRCode/Data_Hash",
        dataType: "json",
        async: false,
        data: requestParams,
        success: function (data) {
            console.log(data);
        },
    });
}
//#endregion

//#region 4.3.5 数据签名接口
function Data_Signature() {
    $.ajax({
        type: "POST",
        url: "/QRCode/Data_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("这个是 4.3.5 数据签名接口结果")
            console.log(data.Data);
            uniqueId = data.Data.uniqueId;
            BackDataString["uniqueId"] = data.Data.uniqueId;
            BackDataString["timestamp"] = data.Data.timeStampSignData;
            BackDataString["stamp"] = data.Data.stamp;
            console.log(BackDataString);
        },
    });
}
//#endregion
//#region 4.3.5 数据签名接口(pdf)
function PDF_Signature() {
    $.ajax({
        type: "POST",
        url: "/QRCode/PDF_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("4.3.5 数据签名接口(pdf)结果")
            uniqueId = data.Data.uniqueId;
            console.log(data.Data);
            BackDataString["uniqueId"] = data.Data.uniqueId;
            BackDataString["timestamp"] = data.Data.timeStampSignData;
            BackDataString["stamp"] = data.Data.stamp;
            console.log(BackDataString);
        },
    });
}
//#endregion
//#region 4.3.5 数据签名接口(电子病历)
function EMR_Signature() {
    $.ajax({
        type: "POST",
        url: "/QRCode/EMR_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("4.3.5 数据签名接口(电子病历)结果");
            if (data.Status == "0") {
                uniqueId = data.Data.uniqueId;
                BackDataString["uniqueId"] = data.Data.uniqueId;
                BackDataString["timestamp"] = data.Data.timeStampSignData;
                BackDataString["stamp"] = data.Data.stamp;
            }
            else {
                var backerrorinfo = {};
                backerrorinfo["id"] = BackDataString["id"] = id;
                backerrorinfo["sign_value"] = data.Message;
                backerrorinfo["sign_state"] = "4";
                showerrorinfo = data.Message;
                $.ajax({
                    type: "POST",
                    url: "/QRCode/BackData",
                    dataType: "json",
                    async: false,
                    data: { datainfo: JSON.stringify(backerrorinfo) },
                    success: function (data) {
                        console.log(data);
                        $("#signsuccess").show();
                        $('#info').text(showerrorinfo);
                        console.log(showerrorinfo);
                        console.log(1111111);
                        return;
                    },
                });
            }
        },
    });
}
//#endregion

//#region 5.1.2获取签名结果
function Get_Signature() {
    $.ajax({
        type: "POST",
        url: "/QRCode/Get_Signature",
        dataType: "json",
        async: false,
        data: { uniqueId: uniqueId },
        success: function (data) {
            console.log("这个是5.1.2获取签名结果")
            console.log(data.Data);
            BackDataString["cert"] = data.Data.cert;
            BackDataString["uniqueId"] = data.Data.uniqueId;
            BackDataString["sign_value"] = data.Data.p1;
            BackDataString["signTime"] = data.Data.signTime;
            BackDataString["sign_state"] = "1"; //代签
            console.log(BackDataString);
            console.log(BackDataString);
            DataBack();
        },
    });
}
//#endregion

//#region (自动签名) 5.1.2获取签名结果

function Auto_Get_Signature() {
    $.ajax({
        type: "POST",
        url: "/QRCode/Get_Signature",
        dataType: "json",
        async: false,
        data: { uniqueId: uniqueId },
        success: function (data) {
            console.log("这个是5.1.2获取签名结果");
            console.log(data.Data);

            if (data.Data.signTime != null) {
                BackDataString["cert"] = data.Data.cert;
                BackDataString["uniqueId"] = data.Data.uniqueId;
                BackDataString["sign_value"] = data.Data.p1;
                BackDataString["signTime"] = data.Data.signTime;
                BackDataString["sign_state"] = "2"; //已经签名
                // BackDataString["sign_state"] = "3"; // 拒签
                console.log(BackDataString);
                DataBack();
            }
            else {
                // 在没有获取到 signedData 时，继续轮询
                pollingTimeout = setTimeout(Auto_Get_Signature, 1000);
            }
        },
    });
}

//#endregion

//#region   证书验证
function CertVail(cert) {
    $.ajax({
        type: "POST",
        url: "/QRCode/CertVail",
        dataType: "json",
        async: false,
        data: { cert: cert },
        success: function (data) {
            console.log(data);
            if (data.status == "200") {
                DocRequestSelect();
            }
        },
    });
}
//#endregion

//#region   3.8  集成时间戳接口调用
function CertVail(oriData) {
    $.ajax({
        type: "POST",
        url: "/QRCode/createAndGetTssInfo",
        dataType: "json",
        async: false,
        data: { oriData: oriData },
        success: function (data) {
            console.log(data);
            if (data.status == "200") {
                DocRequestSelect();
            }
        },
    });
}
//#endregion