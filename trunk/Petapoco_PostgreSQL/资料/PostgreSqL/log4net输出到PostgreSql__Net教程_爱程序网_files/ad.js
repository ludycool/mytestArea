/* �������JS����
 * @Author Alex Lee 
 * @Time   2012-09-24 21:37
 */

var ad = {
	
    //����ҳ�����Ҳ�
    bdDesc_360300:function(){
        document.writeln("<script type=\"text/javascript\">");
        document.writeln("/*360*300�����Ҳ�*/");
        document.writeln("var cpro_id = \"u1308738\";");
        document.writeln("</script>");
        document.writeln("<script src=\"http://cpro.baidustatic.com/cpro/ui/c.js\" type=\"text/javascript\"></script>");
    },

	//GoogleĦ����
	ggDesc_120600:function(){
		document.writeln("<script async src=\"//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js\"></script>");
        document.writeln("<!-- ���Ħ���� -->");
        document.writeln("<ins class=\"adsbygoogle\" style=\"display:inline-block;width:120px;height:600px\" data-ad-client=\"ca-pub-8390049337295239\" data-ad-slot=\"7729735423\"></ins>");
        document.writeln("<script>(adsbygoogle = window.adsbygoogle || []).push({});</script>");
	},

	//BaiduĦ����
    bdDesc_120600:function(){
        document.writeln("<script type=\"text/javascript\">");
        document.writeln("/*120*600��Ϣҳ���*/");
        document.writeln("var cpro_id = \"u1009119\";");
        document.writeln("</script>");
        document.writeln("<script src=\"http://cpro.baidustatic.com/cpro/ui/c.js\" type=\"text/javascript\"></script>");
    },

	//120*600������ѡ��
	desc_120600:function(){
		var _randNum = Math.random();
		if(_randNum <= 0.25){
			this.bdDesc_120600();
		}else{
			this.ggDesc_120600();
		}
	},

	//����ҳ���µײ�
    bdDesc_336280:function(){
        document.writeln("<script type=\"text/javascript\">");
        document.writeln("/*336*280���µײ�*/");
        document.writeln("var cpro_id = \"u599471\";");
        document.writeln("</script>");
        document.writeln("<script src=\"http://cpro.baidustatic.com/cpro/ui/c.js\" type=\"text/javascript\"></script>");
    }

}