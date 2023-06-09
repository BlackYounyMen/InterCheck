//#region  没有登录下的分支

function openid_Auth_Request() {
    $.ajax({
        type: "post",
        url: "/XTQM/Auth_Request",
        async: false,
        success: function (d) {
            var data = JSON.parse(d);
            console.log("令牌值为", data);
            var d = JSON.parse(Params);
            console.log("转换前的值是", Params)
            d["pad_signToken"] = data.data.signDataId;
            BackDataString["pad_signToken"] = data.data.signDataId;
            Params = JSON.stringify(d);
            console.log("转换后的值是", Params);
            // 在指定元素中生成二维码
            var qrcode = new QRCode(document.getElementById("image"), {
                text: data.data.qrCode,
                width: 250,
                height: 250
            });
            $("#image").removeAttr("title");
            $("#countdown").html(120);
            requestId = data.data.signDataId;
            time2 = setInterval(function () {
                var countdownEl = document.getElementById("countdown"); // 获取数据元素
                var icount = parseInt(countdownEl.textContent); // 获取数据并转换为整数
                icount--; // 每秒减少一个
                countdownEl.textContent = icount; // 更新数据元素内容
                if (icount <= 0) { // 如果数据为零
                    clearInterval(time2); // 停止定时器
                    clearInterval(Backtimer); // 停止定时器
                }
            }, 1000);
        },
    });
}

function openid_GetUser() {
    $.ajax({
        type: "post",
        url: "/XTQM/GetUser",
        data: { requestId: requestId },
        async: false,
        success: function (d) {
            var data = JSON.parse(d);
            console.log(new Date, "第" + i++ + "次执行结果，" + data.message);
            if (data.data.msspId != null && data.data.msspId != undefined && data.data.msspId != "" && data.data.jobStatus == "FINISH") {
                openid = data.data.msspId;
                $.ajax({
                    type: "POST",
                    url: "/XTQM/CertVail",
                    dataType: "json",
                    async: false,
                    data: { cert: data.data.signCert },
                    success: function (data) {
                        if (data.status == "200") {
                            Sign();
                        }
                    },
                });
            }
            else {
                Backtimer = setTimeout(() => { openid_GetUser() }, 2000);
            }
        },
    });
}

function Sign() {
    $.ajax({
        type: "POST",
        url: "/XTQM/DocRequestSelect",
        dataType: "json",
        async: false,
        data: { msspId: openid },
        success: function (data) {
            console.log("用户测试，获取到的用户id", data.data);
            console.log("用户测试，前端传出来的用户id", ca_user_id);
            if (data.data.msspId == ca_user_id) {
                layer.msg('登录成功');
                var d = JSON.parse(Params);
                d["appid"] = data.data.msspId;
                d["pad_signtoken"] = BackDataString["pad_signtoken"];

                Params = JSON.stringify(d);

                BackDataString["appid"] = data.data.msspId;

                console.log("传递数据的值是", Params);
                //DocRequestSelect();
                clearTimeout(timer);//清理定时任务
                clearTimeout(Backtimer);//清理定时任务
                clearTimeout(time2);//清理定时任务
                clearTimeout(time1);//清理定时任务

                $("#Display").remove();
                Auto_Sign_Auth();
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
//#endregion

//#endregion

//#region  统一走的方法
function Auto_Sign_Auth() {
    console.log("需要走的签名方法是:", type);

    switch (type) {
        case "1":
            console.log("走1")
            DATA_autoSign();
            break;
        case "2":
            console.log("走2")
            HASH_autoSign();
            break;
        case "3":
            console.log("走3")
            WEB_SEAL_autoSign();
            break;

        default: console.log("数据失误")
    }
}
//#endregion

function DATA_autoSign() {
    $.ajax({
        type: "POST",
        url: "/XTQM/DATA_autoSign",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("结果为:", data);
            if (data.status == "200") {
                console.log("自动签名DATA类型返回结果", data.data);
                var signdataId = data.data.signDataId;
                console.log("签名的编号是", signdataId);
                BackDataString["signdataId"] = signdataId;
                BackDataString["sign_value"] = data.data.signResult;
                GetAuto_Sign(signdataId);
            }
        },
    });
}

function HASH_autoSign() {
    $.ajax({
        type: "POST",
        url: "/XTQM/HASH_autoSign",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("结果为:", data);
            if (data.status == "200") {
                console.log("自动签名HASH类型返回结果", data.data);
                var signdataId = data.data.signDataId;
                console.log("签名的编号是", signdataId);
                BackDataString["signdataId"] = signdataId;
                BackDataString["sign_value"] = data.data.signResult;
                GetAuto_Sign(signdataId);
            }
        },
    });
}

function WEB_SEAL_autoSign() {
    $.ajax({
        type: "POST",
        url: "/XTQM/WEB_SEAL_autoSign",
        dataType: "json",
        async: false,
        data: { databody: Params },
        success: function (data) {
            console.log("结果为:", data);
            if (data.status == "200") {
                console.log("自动签名WEB_SEAL类型返回结果", data.data);
                var signdataId = data.data.signDataId;
                console.log("签名的编号是", signdataId);
                BackDataString["signdataId"] = signdataId;
                BackDataString["sign_value"] = data.data.signResult;
                GetAuto_Sign(signdataId);
            }
        },
    });
}

function createAndGetTssInfo() {
    $.ajax({
        type: "POST",
        url: "/XTQM/createAndGetTssInfo",
        dataType: "json",
        async: false,
        data: { oriData: Params },
        success: function (data) {
            if (data.status == "200") {
                console.log("生成时间戳tsResp", data.data);
                BackDataString["timestamp"] = data.data.tsResp;
                DataBack();
            }
        },
    });
}

//#region  获取签名结果
function GetAuto_Sign(params) {
    // 定义一个函数来执行AJAX请求
    function performAjaxRequest() {
        $.ajax({
            type: "POST",
            url: "/XTQM/Get_Signature",
            dataType: "json",
            data: { signDataId: params },
            success: function (data) {
                console.log("获取签名结果返回数据", data);
                if (data.status == 200 && data.data.jobStatus == "FINISH") {
                    console.log("获取签名结果返回数据", data.data);
                    BackDataString["signcert"] = data.data.signCert;
                    createAndGetTssInfo(Params);
                } else {
                    setTimeout(performAjaxRequest, 2000);
                }
            },
        });
    }
    // 开始最初的AJAX请求
    performAjaxRequest();
}

//#endregion

//#region  返回前端后的结果
function DataBack() {
    console.log(BackDataString);
    $.ajax({
        type: "POST",
        url: "/XTQM/BackData",
        dataType: "json",
        async: false,
        data: { datainfo: JSON.stringify(BackDataString) },
        success: function (data) {
            console.log(data);
            $("#signsuccess").show();
            $('#info').text("签名已成功");
            return;
        },
    });
}