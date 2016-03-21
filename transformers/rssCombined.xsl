<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" />

    <xsl:template match="/">
        <div class="rssItems">
            <xsl:for-each select="rss/channel/item">
                <xsl:variable name="rssDescription">
                    <xsl:call-template name="removeHtmlTags">
                        <xsl:with-param name="html" select="description" />
                    </xsl:call-template>
                </xsl:variable>
                <div class="rssItem">
                    <div class="rssTitle">
                        <a href="{link}">
                            <xsl:value-of select="title" />
                        </a>
                    </div>
                    <div class="rssChannelTitle">
                        <xsl:value-of select="channeltitle"/>
                    </div>
                    <div class="rssBody">
                        <xsl:choose>
                            <xsl:when test="string-length($rssDescription) > 150">
                                <xsl:value-of select="substring($rssDescription, 1, 150)"/>... <a href="{link}">more</a>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="$rssDescription"/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </div>
                </div>
            </xsl:for-each>
        </div>
    </xsl:template>

    <xsl:template name="removeHtmlTags">
        <xsl:param name="html"/>
        <xsl:choose>
            <xsl:when test="contains($html, '&lt;')">
                <xsl:value-of select="substring-before($html, '&lt;')"/>
                <!-- Recurse through HTML -->
                <xsl:call-template name="removeHtmlTags">
                    <xsl:with-param name="html" select="substring-after($html, '&gt;')"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$html"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>