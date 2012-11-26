//倒数时间详细页面
function showTime(overdata, showObjId, isStart) {
    var arr = overdata.split(" ");
    var arr1 = arr[0].split("-");
    var arr2 = arr[1].split(":");
    var endData = new Date(arr1[0], (Number(arr1[1]) - 1), arr1[2], arr2[0], arr2[1], arr2[2]);

    // 剩余总秒
    var total = (endData - new Date()) / 1000;
    if (parseInt(total) <= 0) {
        $("#" + showObjId).text("团购已经结束，可以继续购买");
        clearInterval(showTime);
        return;
    }
    // 计算时间
    var day = parseInt(total / 86400);
    //var hour = parseInt(total / 3600);
    var hour = parseInt(total % 86400 / 3600);
    var min = parseInt((total % 3600) / 60);
    var sec = parseInt((total % 3600) % 60);

    if (hour.toString().length == 1)
        hour = "0" + hour;
    if (min.toString().length == 1)
        min = "0" + min;
    if (sec.toString().length == 1)
        sec = "0" + sec;
    var showTime ="<strong>"+day + "</strong>天<strong>" + hour + "</strong>时<strong>" + min + "</strong>分<strong>" + sec + "</strong>秒";

    // 显示
    if (isStart == "1")
        $("#" + showObjId).html(showTime);
    else
        $("#" + showObjId).html("即将开始");
}

//倒数时间列表页面
function showTimeList(overdata, showObjId, isStart) {
    var arr = overdata.split(" ");
    var arr1 = arr[0].split("-");
    var arr2 = arr[1].split(":");
    var endData = new Date(arr1[0], (Number(arr1[1]) - 1), arr1[2], arr2[0], arr2[1], arr2[2]);

    // 剩余总秒
    var total = (endData - new Date()) / 1000;

    if (parseInt(total) <= 0) {
        $("#" + showObjId).text("团购已经结束");
        return;
    }

    // 计算时间
    var day = parseInt(total / 86400);
    //var hour = parseInt(total / 3600);
    var hour = parseInt(total % 86400 / 3600);
    var min = parseInt((total % 3600) / 60);
    var sec = parseInt((total % 3600) % 60);

    if (hour.toString().length == 1)
        hour = "0" + hour;
    if (min.toString().length == 1)
        min = "0" + min;
    if (sec.toString().length == 1)
        sec = "0" + sec;
    var showTime ="<strong>"+day + "</strong>天<strong>" + hour + "</strong>时<strong>" + min + "</strong>分<strong>" + sec + "</strong>秒";
    // 显示
    if (isStart == "1")
        $("#" + showObjId).html(showTime);
    else
        $("#" + showObjId).html("团购即将开始");
} 
