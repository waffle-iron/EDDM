<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>

    <xsl:template match="/">
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="/*">
        <table border="0" cellpadding="2" cellspacing="0" width="100%" class="datagrid">
            <xsl:for-each select="*[position() = 1]/*">
                <tr>
                    <td style="font-weight:bold;text-align:right;border-right:1px dotted #C0C0C0;">
                        <xsl:value-of select="local-name()"/>
                    </td>
                    <td>
                        <xsl:value-of select="."/>&#160;
                    </td>
                </tr>
            </xsl:for-each>
        </table>
    </xsl:template>

    <!--<xsl:template match="/*/*">
            <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="/*/*/*">
            <xsl:value-of select="."/>
    </xsl:template>-->

</xsl:stylesheet>
