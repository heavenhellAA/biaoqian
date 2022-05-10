//layui 模块化引用
layui.use(['jquery', 'laytpl', 'element'],function(){
    var laytpl = layui.laytpl,
		element = layui.element;
		$ = layui.$;
	
	//获取数据概览的计数统计信息
	var homeTotalData = {sj1: 0, sj2: 0, sj3: 0, total: 0};
	var getHomeTotalData = function(){
		var requestUrl = "..//assets/json/homeTotal.json";  //设置请求接口地址        
        $.get(requestUrl, function(res){
            var data = res && res.code == 0 ? res.data : [];
			console.log(data);
			for (i in data) {
				homeTotalData[i] = data[i];
				$("#rowStatistic").find('.'+ i).text(homeTotalData[i]);
			}			
        });
	}
	
	//获取文章计数信息
	getHomeTotalData();
	
	//内容模板容器与读取模板信息
    var sysConfigView = document.getElementById("sysConfigView"), sysConfigTpl = document.getElementById("sysConfigTpl");
    //渲染内容模板方法
    var renderSysConfigTpl = function (data) {
        if(sysConfigTpl){
            laytpl(sysConfigTpl.innerHTML).render(data, function (render) {
                if(sysConfigView){
                    sysConfigView.innerHTML = render;
                }
            });
        }
    };
	
	//加载系统配置参数数据
	var	sysConfigKey = "sysConfig";	
	var renderSysData = function(){
		if(window.sessionStorage.getItem(sysConfigKey)){
			var data, storage = window.sessionStorage.getItem(sysConfigKey) || "";
			if(storage && storage != "undefined"){
				try{
					data = JSON.parse(storage);
				}catch(err){
					data = eval('(' + storage + ')');
				}finally{
					renderSysConfigTpl(data);
				}
			}
		}else{
			var requestUrl = "../assets/json/sysConfig.json";
			$.get(requestUrl, function(res){
				if(res.code == 0){
					var data = res.data;
					renderSysConfigTpl(data);               
					window.sessionStorage.setItem(sysConfigKey, JSON.stringify(data));
				}
			});
		}
	}
	
	//初始化加载视图数据
	renderSysData(); 
});
