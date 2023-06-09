loadmenu();

function loadmenu() {
    $.ajax({
        type: "get",
        url: "/home/getmenu",
        async: false,
        success: function (menus) {
            var tmphtml = initmenu(menus);
            $('#leftmenu').html(tmphtml);
        }
    })
}

function initmenu(menus) {
    var tmphtml = '';
    for (var i = 0; i < menus.length; i++) {
        if (menus[i].State) {
            if (i == 0) {
                tmphtml += '<li class="layui-nav-item  layui-nav-itemed">';
            }
            else {
                tmphtml += '<li class="layui-nav-item">';
            }
            if (menus[i].Url != '' && menus[i].Url != undefined && menus[i].Url != null) {
                tmphtml += '<a class="site-active" style="cursor: pointer;" tab-url="' + menus[i].Url + '"  tab-name="' + menus[i].Title + '" tab-layid="' + menus[i].Id + '">' + menus[i].Title + '</a>';
            }
            else {
                tmphtml += '<a class="site-active" style="cursor: pointer;">' + menus[i].Title + '</a>';
            }
            var m = menus[i].Childrens.length;
            if (m > 0) {
                tmphtml += '<dl class="layui-nav-child">';
                for (var j = 0; j < m; j++) {
                    if (menus[i].Childrens[j].State) {
                        tmphtml += '<dd><a class="site-active" style="margin-left:20px" tab-name="' + menus[i].Childrens[j].Title + '" tab-Url="' + menus[i].Childrens[j].Url + '" tab-layid="' + menus[i].Childrens[j].Id + '">' + menus[i].Childrens[j].Title + '</a>'
                        var n = menus[i].Childrens[j].Childrens.length;
                        if (n > 0) {
                            tmphtml += '<dl class="layui-nav-child">';
                            for (var x = 0; x < n; x++) {
                                if (menus[i].Childrens[j].Childrens[x].State) {
                                    tmphtml += '<dd><a class="site-active" style="margin-left:40px" tab-name="' + menus[i].Childrens[j].Childrens[x].Title + '" tab-Url="' + menus[i].Childrens[j].Childrens[x].Url + '" tab-layid="' + menus[i].Childrens[j].Childrens[x].Id + '">' + menus[i].Childrens[j].Childrens[x].Title + '</a></dd>'
                                }
                            }
                            tmphtml += '</dl>';
                        }
                        tmphtml += "</dd>"
                    }
                }
                tmphtml += '</dl>';
            }
            tmphtml += '</li>';
        }
    }
    return tmphtml;
}