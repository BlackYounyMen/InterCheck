
var uniqueId;
function YWXStart(d) {
    var data = d.data;
    if (data.type == "2") {
        type = "EMR";
    } else if (data.type == "1") {
        type = "Data";
    }
    //ca_user_id = data.ca_user_id;
    ca_user_id = "130429200303045418";
    backurl = data.backurl;
    Params = JSON.stringify(data);
    backDataString["type"] = data.type;
    backDataString["id"] = id;
    backDataString["openid"] = openId;

    if (openId == null || openId == undefined || openId == "") {
        $("#Display").show();
        $("#Name").html("登录");
        YWX_Login();
        YWX_GetInfo();
    } else {
        Auto_Sign();
    }
}

///手动签名
function Continue() {
    clearInterval(SigntimerSign); // 停止定时器
    clearInterval(SignStatetimer); // 停止定时器
    $("#Display").hide();
    if (type == "EMR") {//纯数据类型
        console.log("这里是手动签名返回的数据")
        YWX_EMR_Sign();
        Get_Signature();
    }
    else if (type == "PDF") {//拥有pdf文件类型
        console.log("这里是手动签名返回的数据")
        YWX_PDF_Sign();
        Get_Signature();
    }
}

//#region 4.3.1自动签名授权-请求自动签名授权接口
function Auto_Sign() {
    $.ajax({
        type: "POST",
        url: "/YWX/Acq_Auth_Res",
        async: false,
        dataType: "json",
        data: { openId: openId },
        success: function (data) {
            if (data.data.grantStep == "1") {
                $("#Display").hide();
                if (type == "Data") {
                    console.log("这是自动签名返回的数据")
                    YWX_Data_Sign();
                    Auto_Get_Signature();
                }
                else if (type == "PDF") {
                    console.log("这是自动签名返回的数据")
                    YWX_PDF_Sign();
                    Auto_Get_Signature();
                }
                else if (type == "EMR") {
                    YWX_EMR_Sign();
                    Auto_Get_Signature();
                }
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/YWX/Auto_Sign_Auth",
                    dataType: "json",
                    async: false,
                    data: { openId: openId },
                    success: function (data) {
                        $("#Display").show();
                        $("#sign").show();
                        $("#image").attr("src", "data:image/png;base64," + data.data);
                        Auto_Sign();
                        time1 = setInterval(function () {
                            var countdownEl = document.getElementById("countdown"); // 获取数据元素
                            var count = parseInt(countdownEl.textContent); // 获取数据并转换为整数
                            count--; // 每秒减少一个
                            countdownEl.textContent = count; // 更新数据元素内容
                            if (count <= "0") { // 如果数据为零
                                clearInterval(time1); // 停止定时器
                            }
                        }, 1000);
                    },
                });
            }
        },
    });
}
//#endregion

//#region   3.3.1 OAuth认证请求接口

function YWX_Login() {
    $.ajax({
        type: "post",
        url: "/YWX/Auth_Request",
        async: false,
        success: function (d) {
            var data = JSON.parse(d);
            console.log(data);

            var divElement = document.getElementById("image");
            var imgElement = document.createElement("img");
            imgElement.src = "data:image/png;base64," + data.data.imgBase64;
            divElement.parentNode.replaceChild(imgElement, divElement);

            // $("#image").attr("src", "data:image/png;base64," + data.data.imgBase64);
            $("#countdown").html(data.data.time);

            requestId = data.data.requestId;
            time2 = setInterval(function () {
                var countdownEl = document.getElementById("countdown"); // 获取数据元素
                var icount = parseInt(countdownEl.textContent); // 获取数据并转换为整数
                icount--; // 每秒减少一个
                countdownEl.textContent = icount; // 更新数据元素内容
                if (icount <= 0) { // 如果数据为零
                    clearInterval(time2); // 停止定时器
                    clearInterval(backtimer); // 停止定时器
                }
            }, 1000);
        },
    });
}

//#endregion

//#region 3.3.2 OAuth登陆-获取用户信息

function YWX_GetInfo() {
    $.ajax({
        type: "post",
        url: "/YWX/GetUser",
        data: { requestId: requestId },
        async: false,
        success: function (d) {
            var data = JSON.parse(d);
            console.log(new Date, "第" + i++ + "次执行结果，" + data.message);
            if (data.status == "0") {
                openId = data.data.uniqueid;

                $.ajax({
                    type: "POST",
                    url: "/YWX/DocRequestSelect",
                    dataType: "json",
                    async: false,
                    data: { openId: openId },
                    success: function (data) {
                        console.log(data.data);
                        if (data.data.userIdcardNum == ca_user_id) {
                            layer.msg('登录成功');
                            var d = JSON.parse(Params);
                            console.log(d);
                            d["openId"] = data.data.openId;
                            console.log(d); 
                            Params = JSON.stringify(d);

                            backDataString["openid"] = data.data.openid;
                            console.log(Params);
                            //DocRequestSelect();
                            clearTimeout(timer);//清理定时任务
                            clearTimeout(backtimer);//清理定时任务
                            clearTimeout(time2);//清理定时任务
                            clearTimeout(time1);//清理定时任务

                            $("#Name").html("授权");
                            $("#sign").show();
                            Auto_Sign();
                        }
                        else {
                            $("#Display").hide();
                            $("#signsuccess").show();
                            $('#info').text("登录账号与app端不匹配");
                            return;
                        }
                    },
                });
            }
            else {
                backtimer = setTimeout(() => { YWX_GetInfo() }, 2000);
            }
        },
    });
}

//#region 4.3.5 数据签名接口
function YWX_Data_Sign() {
    $.ajax({
        type: "POST",
        url: "/YWX/Data_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("这个是 4.3.5 数据签名接口结果")
            console.log(data.data);
            uniqueId = data.data.uniqueId;
            backDataString["uniqueId"] = data.data.uniqueId;
            backDataString["timestamp"] = data.data.timeStampSignData;
            backDataString["stamp"] = data.data.stamp;
            console.log(backDataString);
        },
    });
}
//#endregion
//#region 4.3.5 数据签名接口(pdf)
function YWX_PDF_Sign() {
    $.ajax({
        type: "POST",
        url: "/YWX/PDF_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("4.3.5 数据签名接口(pdf)结果")
            uniqueId = data.Data.uniqueId;
            console.log(data.Data);
            backDataString["uniqueId"] = data.data.uniqueId;
            backDataString["timestamp"] = data.data.timeStampSignData;
            backDataString["stamp"] = data.data.stamp;
            console.log(backDataString);
        },
    });
}
//#endregion
//#region 4.3.5 数据签名接口(电子病历)
function YWX_EMR_Sign() {
    $.ajax({
        type: "POST",
        url: "/YWX/EMR_Signature",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("4.3.5 数据签名接口(电子病历)结果");
            if (data.Status == "0") {
                uniqueId = data.Data.uniqueId;
                backDataString["uniqueId"] = data.data.uniqueId;
                backDataString["timestamp"] = data.data.timeStampSignData;
                backDataString["stamp"] = data.data.stamp;
            }
            else {
                var backerrorinfo = {};
                backerrorinfo["id"] = backDataString["id"] = id;
                backerrorinfo["sign_value"] = data.message;
                backerrorinfo["sign_state"] = "4";
                var showerrorinfo = data.message;              
                $.ajax({
                    type: "POST",
                    url: "/Home/BackData",
                    dataType: "json",
                    async: false,
                    data: { datainfo: JSON.stringify(backerrorinfo), SignatureType: signatureType },
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

//#region (自动签名) 5.1.2获取签名结果

function Auto_Get_Signature() {
    $.ajax({
        type: "POST",
        url: "/YWX/Get_Signature",
        dataType: "json",
        async: false,
        data: { uniqueId: uniqueId },
        success: function (data) {
            console.log("这个是5.1.2获取签名结果");
            console.log(data.data);

            if (data.data.signTime != null) {
                backDataString["cert"] = data.data.cert;
                backDataString["uniqueId"] = data.data.uniqueId;
                backDataString["sign_value"] = data.data.p1;
                backDataString["signTime"] = data.data.signTime;
                backDataString["sign_state"] = "2"; //已经签名
                // backDataString["sign_state"] = "3"; // 拒签
                console.log(backDataString);
                DataBack();
            }
            else {
                // 在没有获取到 signedData 时，继续轮询
                pollingTimeout = setTimeout(Auto_Get_Signature(), 1000);
            }
        },
    });
}