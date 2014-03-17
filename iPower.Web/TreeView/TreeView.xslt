<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dt="urn:schemas-microsoft-com:datatypes">
  <xsl:output method="html" indent="yes"/>
  
	<xsl:template match="/">
    <xsl:element name ="style">
      <xsl:attribute name ="type">text/css</xsl:attribute>
      <![CDATA[
      .TreeViewA { color:#4164b7;	}   
      .TreeViewA:hover { color:"#5674b9";} 
      .TreeViewA:Normal { color:"#7da7d9";}
      .TreeViewCurrentNode{ color: red;}
      .TreeViewTd { float:left; text-align:left; font-size:9pt;vertical-align:bottom;}
      .TreeView img{ width:16px; margin:0 0 0 0;padding:0 0 0 0;display:inline;border:solid 0px red;}
      ]]>
    </xsl:element>
    
		<xsl:apply-templates select="//tree"/>
	</xsl:template>
  
	<xsl:template match="tree">
		<xsl:if test="count(*) &gt; 0">			
			<xsl:variable name="currentfid" select="@currentfid"/>
			<xsl:variable name="checkbox" select="@checkbox"/>
			<xsl:variable name="checktype" select="@checktype"/>
			<xsl:variable name="maxlevel" select="@maxlevel"/>	
			<xsl:variable name="curid" select="@curid"/>							
			<table id="tbl{@id}" cellpadding="0" cellspacing="0" border="0" class="TreeView">
				<xsl:for-each select="folder">
					<xsl:call-template name="folder">
						<xsl:with-param name="currentfid" select="$currentfid"/>
						<xsl:with-param name="checkbox" select="$checkbox"/>
						<xsl:with-param name="checktype" select="$checktype"/>
						<xsl:with-param name="maxlevel" select="$maxlevel"/>
						<xsl:with-param name="curid" select="$curid"/>
					</xsl:call-template>
				</xsl:for-each>
				<tr>
					<xsl:call-template name="lastrow">
						<xsl:with-param name="imagecount" select="$maxlevel"/>
						<xsl:with-param name="curid" select="$curid"/>
					</xsl:call-template>					
				</tr>
			</table>
		</xsl:if>
	</xsl:template>
  
	<!--树表格的最后一行-->
	<xsl:template name="lastrow">
		<xsl:param name="imagecount"/>
		<xsl:param name="curid"/>
		<xsl:if test="$imagecount &gt; 1">
			<td>
        <img>
          <!--src="/Tree/Image/blank.png">-->
          <xsl:attribute name ="src">
            <xsl:value-of select ="//resource/blankUrl"/>
          </xsl:attribute>
          <xsl:if test="$imagecount=2 and $curid!=''">
            <xsl:attribute name="onload">
              javascript:window.setTimeout("var obj=document.all('<xsl:value-of select="$curid"/>');if (obj!=null){obj.scrollIntoView(false);}",100);
            </xsl:attribute>
          </xsl:if>
        </img>
			</td>
			<xsl:variable name="vtmp" select="number($imagecount)-2"/>
			<xsl:call-template name="lastrow">
				<xsl:with-param name="imagecount" select="$vtmp"/>
				<xsl:with-param name="curid" select="$curid"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$imagecount=1">
			<td>
				<xsl:attribute name="width">100%</xsl:attribute>				
			</td>			
		</xsl:if>
	</xsl:template>
  
	<!--画树-->
	<xsl:template name="folder">
		<xsl:param name="pstatus" select="'expand'"/>
		<xsl:param name="currentfid"/>
		<xsl:param name="checkbox"/>
		<xsl:param name="checktype"/>
		<xsl:param name="maxlevel"/>
		<xsl:param name="curid"/>
		<xsl:variable name="fnode" select="current()"/>
		<tr status="{@status}" childrencount="{count(*)}" nodeid="{@id}" pnodeid="{@pid}">
			<xsl:if test="$pstatus != 'expand'">
				<xsl:attribute name="style">display:none;</xsl:attribute>
			</xsl:if>
			<xsl:if test="count(*)>0">
				<!--<xsl:attribute name="onclick">javascript:TreeView_NodeClick();</xsl:attribute>-->
        <xsl:attribute name ="onclick">
          <xsl:text>javascript:TreeView_NodeClick('</xsl:text>
          <xsl:value-of select ="//resource/TplusUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/TminusUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/LplusUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/LminusUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/closedfolderUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/openfolderUrl"/>
          <xsl:text>','</xsl:text>
          <xsl:value-of select ="//resource/XsltUrl"/>
          <xsl:text>');</xsl:text>
        </xsl:attribute>
			</xsl:if>	
			<td nowrap="true">
				<xsl:attribute name="colspan">
          <xsl:number value="number($maxlevel)-number(@level)"/>
        </xsl:attribute>
				<!--画线-->
				<xsl:call-template name="genTreeLine"/>
				<!--目录图标-->
				<xsl:choose>
					<xsl:when test="@status = 'expand'">
						<!--<img src="/Tree/Image/openfolder.gif" border="0" id="img{@id}"/>-->
              <xsl:element name ="img">
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/openfolderUrl"/>
              </xsl:attribute>
              <xsl:attribute name ="border">0</xsl:attribute>
              <xsl:attribute name ="id">
                <xsl:text>img</xsl:text>
                <xsl:value-of select ="@id"/>
              </xsl:attribute>
            </xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<!--<img src="/Tree/Image/closedfolder.gif" border="0" id="img{@id}"/>-->
              <xsl:element name ="img">
                <xsl:attribute name ="src">
                  <xsl:value-of select ="//resource/closedfolderUrl"/>
                </xsl:attribute>
                <xsl:attribute name ="border">0</xsl:attribute>
                <xsl:attribute name ="id">
                  <xsl:text>img</xsl:text>
                  <xsl:value-of select ="@id"/>
                </xsl:attribute>
              </xsl:element>
					</xsl:otherwise>
				</xsl:choose>
			</td>
			<!--选择框-->
			<xsl:if test="$checkbox='true'">
				<td>
					<xsl:choose>
						<xsl:when test="$checktype='checkbox'">
							<input type="checkbox" value="{@value}" onclick="TreeView_CheckAll(this);TreeView_UnCheck(this);">
								<xsl:attribute name="id"><xsl:value-of select="//tree/@id"/>cb<xsl:value-of select="@id"/></xsl:attribute>
								<xsl:attribute name="name"><xsl:value-of select="//tree/@id"/>cb<xsl:value-of select="@id"/></xsl:attribute>
								<xsl:if test="@checked != 'false'">
									<xsl:attribute name="checked">checked</xsl:attribute>
								</xsl:if>
							</input>
						</xsl:when>
						<xsl:otherwise>
							<input type="radio" value="{@value}">
								<xsl:attribute name="id"><xsl:value-of select="//tree/@id"/>rd</xsl:attribute>
								<xsl:attribute name="name"><xsl:value-of select="//tree/@id"/>rd</xsl:attribute>
								<xsl:if test="@checked != 'false'">
									<xsl:attribute name="checked">checked</xsl:attribute>
								</xsl:if>
							</input>
						</xsl:otherwise>
					</xsl:choose>
				</td>
			</xsl:if>
			<td nowrap="true" class="TreeViewTd" colspan="{@level}">
				<!--图片与文字之间有一个空格				<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>-->
				<xsl:if test="@id = $currentfid ">																							
					<xsl:attribute name="id"><xsl:value-of select="$curid"/></xsl:attribute>
					<xsl:attribute name="name"><xsl:value-of select="$curid"/></xsl:attribute>															
				</xsl:if>
				<xsl:choose>
					<xsl:when test="@href !='' or   @clickaction != '' or  @dblclickaction != ''">
						<a class="TreeViewA">
							<xsl:choose>
								<xsl:when test="@href =''  ">
									<xsl:attribute name="href">#</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="href"><xsl:value-of select="@href"/></xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:if test="@clickaction != '' ">
								<xsl:attribute name="onclick"><xsl:value-of select="@clickaction"/></xsl:attribute>
							</xsl:if>
							<xsl:if test="@dblclickaction != '' ">
								<xsl:attribute name="ondblclick"><xsl:value-of select="@dblclickaction"/></xsl:attribute>
							</xsl:if>

							<xsl:choose>
							<xsl:when test="@id = $currentfid ">
								<span class="TreeViewCurrentNode">																										
									<xsl:value-of select="@title"/>
								</span>								
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@title"/>
							</xsl:otherwise>
							</xsl:choose>
						</a>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="@id = $currentfid ">
								<span class="TreeViewCurrentNode">
								<xsl:value-of select="@title"/></span>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@title"/>
							</xsl:otherwise>
							</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>				
			</td>
		</tr>
		<xsl:choose>
			<xsl:when test="$pstatus != 'expand'">
				<xsl:for-each select="folder">
					<xsl:call-template name="folder">
						<xsl:with-param name="pstatus" select="'close'"/>
						<xsl:with-param name="currentfid" select="$currentfid"/>
						<xsl:with-param name="checkbox" select="$checkbox"/>
						<xsl:with-param name="checktype" select="$checktype"/>
						<xsl:with-param name="maxlevel" select="$maxlevel"/>
						<xsl:with-param name="curid" select="$curid"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="folder">
					<xsl:call-template name="folder">
						<xsl:with-param name="pstatus" select="$fnode/@status"/>
						<xsl:with-param name="currentfid" select="$currentfid"/>
						<xsl:with-param name="checkbox" select="$checkbox"/>
						<xsl:with-param name="checktype" select="$checktype"/>
						<xsl:with-param name="maxlevel" select="$maxlevel"/>
						<xsl:with-param name="curid" select="$curid"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
  
	<!--画树线的模版-->
	<xsl:template name="genTreeLine">
		<xsl:for-each select="ancestor::*[name() = 'folder']">
			<xsl:choose>
				<xsl:when test="following-sibling::node()">
					<!--<img src="/Tree/Image/I.gif"/>-->
          <xsl:element name ="img">
            <xsl:attribute name ="src">
              <xsl:value-of select ="//resource/IUrl"/>
            </xsl:attribute>
          </xsl:element>
				</xsl:when>
				<xsl:otherwise>
					<!--<img src="/Tree/Image/blank.png"/>-->
          <xsl:element name ="img">
            <xsl:attribute name ="src">
              <xsl:value-of select ="//resource/blankUrl"/>
            </xsl:attribute>
          </xsl:element>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		<xsl:choose>
			<xsl:when test="count(*) > 0">
				<xsl:choose>
					<xsl:when test="following-sibling::*">
						<img>
							<xsl:attribute name="ID">stateImagef<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:choose>
								<xsl:when test="@status = 'expand'">
									<!--<xsl:attribute name="src">/Tree/Image/Tminus.gif</xsl:attribute>-->
                  <xsl:attribute name ="src">
                    <xsl:value-of select ="//resource/TminusUrl"/>
                  </xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<!--<xsl:attribute name="src">/Tree/Image/Tplus.gif</xsl:attribute>-->
                  <xsl:attribute name ="src">
                    <xsl:value-of select ="//resource/TplusUrl"/>
                  </xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</img>
					</xsl:when>
					<xsl:otherwise>
						<img>
							<xsl:attribute name="ID">stateImagef<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:choose>
								<xsl:when test="@status = 'expand'">
									<!--<xsl:attribute name="src">/Tree/Image/Lminus.gif</xsl:attribute>-->
                  <xsl:attribute name ="src">
                    <xsl:value-of select ="//resource/TminusUrl"/>
                  </xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<!--<xsl:attribute name="src">/Tree/Image/Lplus.gif</xsl:attribute>-->
                  <xsl:attribute name ="src">
                    <xsl:value-of select ="//resource/TplusUrl"/>
                  </xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</img>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="following-sibling::node()">
						<!--<img src="/Tree/Image/T.gif"/>-->
            <xsl:element name ="img">
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/TUrl"/>
              </xsl:attribute>
            </xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<!--<img src="/Tree/Image/L.gif"/>-->
            <xsl:element name ="img">
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/LUrl"/>
              </xsl:attribute>
            </xsl:element>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
