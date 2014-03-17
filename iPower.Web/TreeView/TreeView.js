function TreeView_NodeClick(TplusUrl, TminusUrl, LplusUrl, LminusUrl, closefolderUrl, openfolderUrl,xslt)
{	
	var o=event.srcElement;	
	var CurrentNodeRow;
	//alert(o.tagName);	
	if (o.tagName=="IMG")
	{
		CurrentNodeRow=o.parentElement.parentElement;
	}else if (o.tagName=="TD")
	{
		CurrentNodeRow=o.parentElement;
	}else
	{
		return;
	}	
	
	var oStateImagef=null;
	var oImg=eval("document.all.img"+CurrentNodeRow.nodeid);

	//alert(o.id);
	if(o.id!="undefined" && o.id.indexOf("stateImagef")>-1)
	{
		oStateImagef=o;
	}
	else
	{
		oStateImagef=eval("document.all.stateImagef"+CurrentNodeRow.nodeid);
	}
	
	var isCloseAction= (CurrentNodeRow.status == 'expand');
	
	TreeView_ShowHideChildren(CurrentNodeRow,isCloseAction,xslt);

	TreeView_ChangeImg(oStateImagef, TplusUrl, TminusUrl, LplusUrl, LminusUrl)
	
	TreeView_ChangeRowStatus(CurrentNodeRow);
	TreeView_ChangeFolderImg(oImg, closefolderUrl, openfolderUrl);
}

//设置被点击的节点行的开关状态

function TreeView_ChangeRowStatus(NodeRow)
{
	if(NodeRow.status!="expand")
	{
		NodeRow.status="expand"
	}
	else
	{
		NodeRow.status="close"
	}
}

//改变被点击的节点的节点图

function TreeView_ChangeImg(eImg, TplusUrl, TminusUrl, LplusUrl, LminusUrl)
{
	if(eImg=="undefined" || eImg==null)
	{
		return ;
	}
	
	var sTmpSrc=eImg.src
	if(sTmpSrc=="undefined" || sTmpSrc==null)
	{
		return ;
	}
		
	sTmpSrc="/" + sTmpSrc.substr(sTmpSrc.lastIndexOf("/")).replace("/","")

	//alert(sTmpSrc + "\r\n" + TplusUrl + "\r\n" + TminusUrl + "\r\n" + LplusUrl + "\r\n" + LminusUrl);
	if(sTmpSrc == TminusUrl)//if(sTmpSrc == "Tminus.gif")
	{
	    //eImg.src = "/Tree/Image/Tplus.gif"
	    eImg.src = TplusUrl;
		return
    }
    if(sTmpSrc == TplusUrl)//if(sTmpSrc == "Tplus.gif")
	{
	    //eImg.src = "/Tree/Image/Tminus.gif"
	    eImg.src = TminusUrl;
		return
	}

	if(sTmpSrc == TminusUrl)//if(sTmpSrc == "Lminus.gif")
	{
	    //eImg.src = "/Tree/Image/Lplus.gif"
	    eImg.src = LplusUrl;
		return
	}

	if(sTmpSrc == LplusUrl)//if(sTmpSrc == "Lplus.gif")
	{
	    //eImg.src = "/Tree/Image/Lminus.gif"
	    eImg.src = LminusUrl;
		return
	}
}

	//目录图标
	
	function TreeView_ChangeFolderImg(oImg, closefolderUrl, openfolderUrl)
	{
		if(oImg!=null && oImg.src!=null)
		{
			var s=oImg.src;
			//alert(oImg.src);
			s=s.substr(s.lastIndexOf("/")).replace("/","");
			if(s=="openfolder.gif")
			{
			    //oImg.src="/Tree/Image/closedfolder.gif";
			    oImg.src = closefolderUrl;
			}
			else
			{
			    //oImg.src="/Tree/Image/openfolder.gif";
			    oImg.src = openfolderUrl;
			}
		}
	}

	//自动隐藏或显示相应的节点的子节点
	
	function TreeView_ShowHideChildren(row,isCloseAction,xslt)
	{		
		var length; //堆栈数组长度+1
		var arr=new Array();
		var stat=new Array();
		var cur=row.nextSibling;
		//恢复Tree数据
		if (!isCloseAction)
		{
			RetrieveTreeData(row,xslt);
		}
		
		if (cur && cur.pnodeid==row.nodeid)
		{
			length=0;
			stat[0]="expand";
			arr[0]=row.nodeid; 
		}
		else
		{
			length=-1;
		}
				
		while (cur && length>-1)
		{		
			if (isCloseAction)
			{
				cur.style.display="none";				
			}
			else if (stat[length]=="expand")
			{									
				cur.style.display = "block";
			}
					
			next=cur.nextSibling;
			if (next && next.pnodeid==cur.nodeid)
			{
				length++;
				arr[length]=cur.nodeid;
				stat[length]=cur.status;
			}
			else
			{
				while (next && length>-1)
				{
					if (next.pnodeid==arr[length])
					{
						break;
					}
					length--;
				}
			}		
			cur=next;
		}
	}

//全部关闭

function TreeView_CloseAll(nodeid)
{
	var tbl=event.srcElement.parentElement.parentElement.parentElement;
	var r;
	for(var i=0;i<tbl.rows.length;i++)
	{
		r=tbl.rows[i];
		if (r.pnodeid=='' && r.nodeid!=nodeid)
		{
			var oStateImagef=eval("document.all.stateImagef"+r.nodeid);
			if(oStateImagef!='undefined' && oStateImagef!=null)
			{
				//alert(oStateImagef);
				var s=oStateImagef.src;
				//alert(s);
				if(s!=null || s!='undefined')
				{
					s=s.substr(s.lastIndexOf("/")).replace("/","");
					if(s=="Lplus.gif"||s=="Tminus.gif")
					{
						oStateImagef.click();
					}
				}
			}
		}
	}
}

	function TreeView_CheckAll(oCheckBox)
	{
		var checkid=oCheckBox.id;
		var reg=new RegExp("^"+oCheckBox.id+"(_\\d+)*$","im");
		var bChecked=oCheckBox.checked;
		var checks=oCheckBox.parentElement.parentElement.parentElement.getElementsByTagName("INPUT");		
		var index=0;	
		var level=0;
		var step=checks.length-1;		
		//快速定位 （有序）			
		while(checks[index].id!=checkid && level++<30)
		{								
			if (checks[index].id<checkid)
			{
				index=index+step;
			}
			else
			{
				index=index-step;
			}		
				
			if (step>3)
			{
				step=parseInt(step/2);
			}
			else
			{
				step=1;
			}
		}
		
		step=checks.length;			
		while(index<step && reg.test(checks[index].id))
		{			
			checks[index++].checked=bChecked;		
		}				
	}

	function TreeView_UnCheck(oCheckBox)
	{
		if(oCheckBox.checked==true)
		{
			return false;
		}
		//var reg=new RegExp("^cb((_\\d+)*)?_\\d+$");			
		var tblID=oCheckBox.parentElement.parentElement.parentElement.parentElement.id;
		if(tblID!="")
		{
			tblID=tblID.substring(3);
		}
		//alert(tblID);
		var reg=new RegExp("^"+tblID+"cb((_\\d+)*)?_\\d+$");	
		var obj=document.getElementById(tblID+"cb" + reg.exec(oCheckBox.id)[1]);
		if (obj)
		{
			obj.checked=false;
			TreeView_UnCheck(obj);
		}
	}
	
//	function LoadTreeData(id,key)
//	{
//		var doc = new ActiveXObject("Msxml2.DOMDocument.4.0");
//        doc.async=false;        
//        doc.load("/GetPickerValue.aspx?PickerKey="+key); 

//        var xslt= new ActiveXObject("Msxml2.DOMDocument.4.0");
//        xslt.async = false;        
//        xslt.load("/Tree/TreeView.xslt");
//        alert(doc.xml);        
//		//id.innerHTML = doc.transformNode(xslt);		
//		id.value=doc.xml;		
//	}
	
	function RetrieveTreeData(row, xslt)
	{	
		return;
		var doc=new ActiveXObject("Msxml2.DOMDocument.4.0");
		var div=row.parentElement.parentElement.parentElement;		
		doc.loadXML(div.value);		
		var nod=doc.documentElement.selectSingleNode("//folder[@id=\""+row.nodeid+"\"]");	
		if (nod!=null)
		{
			nod.setAttribute("status","expand");
		}
		var nods=doc.documentElement.selectNodes("//folder[@pid=\""+row.nodeid+"\"]");
		if (nods!=null)
		{
			for	(var i=0;i<nods.length;i++)
			{
				nods[i].setAttribute("status","expand");
			}
		}		
		var xslt= new ActiveXObject("Msxml2.DOMDocument.4.0");
        xslt.async = false;
        //xslt.load("/Tree/TreeView.xslt");
        xslt.load(xslt);          
		div.innerHTML = doc.transformNode(xslt);				
		div.value=doc.xml;
	}
	
//	function SaveTreeData(id,url,value)
//	{
//		var checks=id.getElementsByTagName("INPUT");
//		var doc=new ActiveXObject("Msxml2.DOMDocument");
//		var root=doc.createElement("tree");			
//		var t=doc.createElement("values");
//				
//		var len=checks.length;
//		for(var i=0;i<len;i++)
//		{
//			if (checks[i].checked)
//			{
//				var v=doc.createElement("v");
//				v.text=checks[i].value;
//				t.appendChild(v);
//			}
//		}
//		root.appendChild(t);
//		var ex=doc.createElement("extra");
//		ex.text=value;
//		root.appendChild(ex);		
//		doc.documentElement=root;
//		var req = new ActiveXObject("Msxml2.XMLHTTP");	
//		req.open("POST",url, false);		
//		req.send(doc);
//		return req.responseText;		
//	}