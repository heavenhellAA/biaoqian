//layui 模块化引用
layui.config({
	base : "assets/extends/"
}).use(['jquery', 'layer', 'navTree'],function(){
	var layer = layui.layer, 
		navTree = layui.navTree,
		$ = layui.jquery;	//使用 layui 内部的 jQuery
	
	//隐藏与显示左侧导航
	$(".hideMenu").on("click", function(){
		$("body").toggleClass("layout-beta");
        window.sessionStorage.setItem("layout-beta", $("body").hasClass("layout-beta"));
	});

	//清除缓存	
	$(".clearCache").on("click", function(){		
		var layerIndex = layer.msg('清除缓存中，请稍候…', {icon: 16, time:false, shade:0.8});
		var localData = window.localStorage, sessionData = window.sessionStorage;
		for(var i=0; i<localData.length; i++){
			var localKey = localData.key(i) || "";
			var notClearKeys = ["current_nav"].join(",");
			if(notClearKeys.indexOf(localKey) == -1){
				window.localStorage.removeItem(localKey);
			}
		}
		for(var j=0; j<sessionData.length; j++){
			var sessionKey = sessionData.key(j) || "";			
			var notClearSessionKeys = ["current_user", "current_user_name"].join(",");
			if(notClearSessionKeys.indexOf(sessionKey) == -1){				
				window.sessionStorage.removeItem(sessionKey);
			}
		}
        window.setTimeout(function(){
            layer.close(layerIndex);
            layer.msg("缓存清除成功！");
        }, 1000);
	});

	//退出系统
	$(".logout").on("click", function(){
		var layerIndex = layer.msg('安全退出中，请稍候', {icon: 16, time: false});
		//发送服务端退出系统请求
        $.get("api/logout", function(res) {
			if(res){
				console.log("退出成功，将跳转至登录页");
			} else{
				console.log("退出系统失败，退出过程中发生异常");
			}
        }).fail(function () {
            console.log("退出系统失败，退出过程中发生异常");
        }).always(function () {            
			layer.close(layerIndex);
			window.goLogin();
        });
	});	
	
	//手机设备的简单适配
    $('.layout-mobile-aside').on('click', function(){
		$('body').addClass('layout-mobile-beta');
		return false;
	});
    $('.layout-mobile-shadow').on('click', function(){
		$('body').removeClass('layout-mobile-beta');
		return false;
	});
		
	//窗口重置方法
	var winResize = function() {
		var docWidth = $(window).width();
		if (docWidth > 767) {
			$("body").removeClass("layout-beta");
		} else {
			$("body").addClass("layout-beta");			
			
			//当在小屏幕时，点击左侧菜单链接，触发 .layout-mobile-shadow 遮罩关闭
			$("body").on("click", "a.navHyperLink", function(){
				$('.layout-mobile-shadow').trigger("click");
			});			
		}
	};	
	//页面初始载入时，适应窗口大小
	winResize();
	
	//页面尺寸改变时，适应窗口大小
    $(window).on("resize", function () {        
        window.setTimeout(function () {
            winResize();
        }, 500);	//延迟执行,防止多次触发
	});
	
	var dateNow = new Date();
	$(".thisYear").text(dateNow.getFullYear());
});

//扩展一个方法，当登录超时或未登录时，请重新登录（将清除本地存储并跳转至登录页）
window.goLogin = function(loginPage) {
	loginPage = loginPage || com.basePath + "/login.html";
	window.sessionStorage.clear();	//清除本地会话缓存
	window.localStorage.clear();	//青春本地存储
	window.setTimeout(function(){
		if(window.location.pathname.indexOf(loginPage) == -1) {	//判断当前页面不是登录页
			window.top.location.href = loginPage; //开启登录跳转
		}
	}, 1000);
};