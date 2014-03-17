<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
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
      .TreeView img{ width:16px; margin:0;padding:0;display:inline;border:solid 0px red;}
      ]]>
    </xsl:element>
    
    <!--构建树-->
    <xsl:apply-templates select="//tree"/>
  </xsl:template>

  <!--创建树-->
  <xsl:template match="tree">
    <xsl:if test="count(*) &gt; 0">
      <xsl:variable name="currentfid" select="@currentfid"/>
      <xsl:variable name="checkbox" select="@checkbox"/>
      <xsl:variable name="checktype" select="@checktype"/>
      <xsl:variable name="maxlevel" select="@maxlevel"/>
      <xsl:variable name="curid" select="@curid"/>
      <table id="tbl{@id}" cellpadding="0" cellspacing="0" border="1" class="TreeView">
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
            <xsl:with-param name="count">
              <xsl:choose>
                <xsl:when test="$checkbox='true'">
                  <xsl:value-of select ="$maxlevel + 2"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$maxlevel + 1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
          </xsl:call-template>
        </tr>
      </table>
    </xsl:if>
  </xsl:template>

  <!--画行-->
  <xsl:template name ="folder">
    <xsl:param name="pstatus" select="'expand'"/>
    <xsl:param name="currentfid"/>
    <xsl:param name="checkbox"/>
    <xsl:param name="checktype"/>
    <xsl:param name="maxlevel"/>
    <xsl:param name="curid"/>
    <xsl:variable name="fnode" select="current()"/>
    <tr status="{@status}" childrencount="{count(*)}" nodeid="{@id}" pnodeid="{@pid}">
      <!--是否展开-->
      <xsl:if test="$pstatus != 'expand'">
        <xsl:attribute name="style">display:none;</xsl:attribute>
      </xsl:if>
      <!--单击事件-->
      <xsl:if test="count(*)>0">
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
        <xsl:if test="@level &gt; 1">
          <xsl:attribute name ="colspan">
            <xsl:value-of select="@level"/>
          </xsl:attribute>
        </xsl:if>
        <!--画线-->
        <xsl:call-template name="genTreeLine"/>
        <!--目录图标-->
        <xsl:choose>
          <xsl:when test="@status = 'expand'">
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
                <xsl:attribute name="id">
                  <xsl:value-of select="//tree/@id"/>
                  <xsl:text>cb</xsl:text>
                  <xsl:value-of select="@id"/>
                </xsl:attribute>
                <xsl:attribute name="name">
                  <xsl:value-of select="//tree/@id"/>
                  <xsl:text>cb</xsl:text>
                  <xsl:value-of select="@id"/>
                </xsl:attribute>
                <xsl:if test="@checked != 'false'">
                  <xsl:attribute name="checked">checked</xsl:attribute>
                </xsl:if>
              </input>
            </xsl:when>
            <xsl:otherwise>
              <input type="radio" value="{@value}">
                <xsl:attribute name="id">
                  <xsl:value-of select="//tree/@id"/>
                  <xsl:text>rd</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="name">
                  <xsl:value-of select="//tree/@id"/>
                  <xsl:text>rd</xsl:text>
                </xsl:attribute>
                <xsl:if test="@checked != 'false'">
                  <xsl:attribute name="checked">checked</xsl:attribute>
                </xsl:if>
              </input>
            </xsl:otherwise>
          </xsl:choose>
        </td>
      </xsl:if>
      
      <td class="TreeViewTd">
        <xsl:variable name="colspanInt">
          <xsl:number value="number($maxlevel)-number(@level)+1"/>
        </xsl:variable>
        <xsl:if test="$colspanInt &gt;1">
          <xsl:attribute name="colspan">
            <xsl:value-of select ="$colspanInt"/>
          </xsl:attribute>
        </xsl:if>

        <xsl:if test="@id = $currentfid ">
          <xsl:attribute name="id">
            <xsl:value-of select="$curid"/>
          </xsl:attribute>
          <xsl:attribute name="name">
            <xsl:value-of select="$curid"/>
          </xsl:attribute>
        </xsl:if>
        
        <xsl:choose>
          <xsl:when test="@href !='' or   @clickaction != ''">
            <a class="TreeViewA">
              <xsl:choose>
                <xsl:when test="@href =''  ">
                  <xsl:attribute name="href">#</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="href">
                    <xsl:value-of select="@href"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:if test="@clickaction != '' ">
                <xsl:attribute name="onclick">
                  <xsl:value-of select="@clickaction"/>
                </xsl:attribute>
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
                  <xsl:value-of select="@title"/>
                </span>
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

  <!--画树线-->
  <xsl:template name="genTreeLine">
    <!--如果有与父同级的-->
    <xsl:for-each select="ancestor::*[name() = 'folder']">
      <xsl:choose>
        <xsl:when test="following-sibling::node()">
           <xsl:element name ="img">
            <xsl:attribute name ="src">
              <xsl:value-of select ="//resource/IUrl"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:when>
        <xsl:otherwise>
           <xsl:element name ="img">
            <xsl:attribute name ="src">
              <xsl:value-of select ="//resource/blankUrl"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    
    <!--本级-->
    <xsl:choose>
      <xsl:when test="count(*) > 0">
        <img>
          <xsl:attribute name="ID">
            <xsl:text>stateImagef</xsl:text>
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="@status = 'expand'">
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/TminusUrl"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/TplusUrl"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </img>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="following-sibling::node()">
            <xsl:element name ="img">
              <xsl:attribute name ="src">
                <xsl:value-of select ="//resource/TUrl"/>
              </xsl:attribute>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
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


  <!--树表格的最后一行-->
  <xsl:template name="lastrow">
    <xsl:param name="count"/>
    <xsl:if test="$count &gt; 1">
      <td>
        <br/>
      </td>
      <xsl:call-template name ="lastrow">
        <xsl:with-param name ="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:if test="$count=1">
      <td>
        <br/>
      </td>
    </xsl:if>
  </xsl:template>
    
</xsl:stylesheet>
