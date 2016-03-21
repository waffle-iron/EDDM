<?xml version="1.0"?>
<!-- Stylesheet for creating ReportViewer RDLC documents -->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"
  xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"  xmlns="http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition"
  >

	<xsl:variable name="mvarName" select="/xs:schema/@Name"/>
	<xsl:variable name="mvarFontSize">8pt</xsl:variable>
	<xsl:variable name="mvarFontWeight">500</xsl:variable>
	<xsl:variable name="mvarFontWeightBold">700</xsl:variable>


	<xsl:template match="/">
		<xsl:apply-templates select="/xs:schema/xs:element/xs:complexType/xs:choice/xs:element/xs:complexType/xs:sequence">
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="xs:sequence">
		<Report xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner" xmlns="http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition">
			<xsl:call-template name="BuildDataSource">
			</xsl:call-template>
			<InteractiveHeight>11in</InteractiveHeight>
			<rd:DrawGrid>true</rd:DrawGrid>
			<InteractiveWidth>8.5in</InteractiveWidth>
			<rd:SnapToGrid>true</rd:SnapToGrid>
			<RightMargin>1in</RightMargin>
			<LeftMargin>1in</LeftMargin>
			<BottomMargin>1in</BottomMargin>
			<TopMargin>1in</TopMargin>
			<Language>en-US</Language>
			<rd:ReportID>7358b654-3ca3-44a0-8677-efe0a55c7c45</rd:ReportID>
			<xsl:call-template name="BuildDataSet">
			</xsl:call-template>
			<Width>6.5in</Width>
			<Body>
				<ReportItems>
					<Table Name="table1">
						<ZIndex>1</ZIndex>
						<DataSetName>
							<xsl:value-of select="$mvarName" />
						</DataSetName>
						<Top>0.5in</Top>
						<Height>0.50in</Height>
						<Details>
							<TableRows>
								<TableRow>
									<Height>0.25in</Height>
									<TableCells>

										<xsl:apply-templates select="xs:element" mode="DetailTableCell">
										</xsl:apply-templates>

									</TableCells>
								</TableRow>
							</TableRows>
						</Details>
						<Header>
							<TableRows>
								<TableRow>
									<Height>0.25in</Height>
									<TableCells>

										<xsl:apply-templates select="xs:element" mode="HeaderTableCell">
										</xsl:apply-templates>

									</TableCells>
								</TableRow>
							</TableRows>
						</Header>
						<TableColumns>
							<xsl:apply-templates select="xs:element" mode="TableColumn">
							</xsl:apply-templates>
						</TableColumns>
					</Table>
				</ReportItems>
				<Height>1.25in</Height>
			</Body>
		</Report>
	</xsl:template>

	<xsl:template name="BuildDataSource">
		<DataSources>
			<DataSource Name="DummyDataSource">
				<ConnectionProperties>
					<ConnectString/>
					<DataProvider>SQL</DataProvider>
				</ConnectionProperties>
				<rd:DataSourceID>84635ff8-d177-4a25-9aa5-5a921652c79c</rd:DataSourceID>
			</DataSource>
		</DataSources>
	</xsl:template>

	<xsl:template name="BuildDataSet">
		<DataSets>
			<DataSet Name="{$mvarName}">
				<Query>
					<rd:UseGenericDesigner>true</rd:UseGenericDesigner>
					<CommandText/>
					<DataSourceName>DummyDataSource</DataSourceName>
				</Query>
				<Fields>
					<xsl:apply-templates select="xs:element" mode="Field">
					</xsl:apply-templates>
				</Fields>
			</DataSet>
		</DataSets>
	</xsl:template>

	<xsl:template match="xs:element" mode="Field">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>

		<xsl:variable name="varDataType">
			<xsl:choose>
				<xsl:when test="@type='xs:int'">System.Int32</xsl:when>
				<xsl:when test="@type='xs:string'">System.String</xsl:when>
				<xsl:when test="@type='xs:dateTime'">System.DateTime</xsl:when>
				<xsl:when test="@type='xs:boolean'">System.Boolean</xsl:when>
				<xsl:otherwise>System.String</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Field Name="{$varFieldName}">
			<DataField>
				<xsl:value-of select="$varFieldName"/>
			</DataField>
			<rd:TypeName>
				<xsl:value-of select="$varDataType"/>
			</rd:TypeName>
		</Field>
	</xsl:template>

	<xsl:template match="xs:element" mode="HeaderTableCell">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>

		<TableCell>
			<ReportItems>
				<Textbox Name="textbox{position()}">
					<rd:DefaultName>textbox<xsl:value-of select="position()"/></rd:DefaultName>
					<Value><xsl:value-of select="$varFieldName"/></Value>
					<CanGrow>true</CanGrow>
					<ZIndex>7</ZIndex>
					<Style>
						<TextAlign>Center</TextAlign>
						<PaddingLeft>2pt</PaddingLeft>
						<PaddingBottom>2pt</PaddingBottom>
						<PaddingRight>2pt</PaddingRight>
						<PaddingTop>2pt</PaddingTop>
						<FontWeight>
							<xsl:value-of select="$mvarFontWeightBold"/>
						</FontWeight>
					</Style>
				</Textbox>
			</ReportItems>
		</TableCell>
	</xsl:template>

	<xsl:template match="xs:element" mode="DetailTableCell">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>

		<TableCell>
			<ReportItems>
				<Textbox Name="{$varFieldName}">
					<rd:DefaultName><xsl:value-of select="$varFieldName"/></rd:DefaultName>
					<Style>
						<TextAlign>Left</TextAlign>
						<PaddingLeft>2pt</PaddingLeft>
						<PaddingBottom>2pt</PaddingBottom>
						<PaddingRight>2pt</PaddingRight>
						<PaddingTop>2pt</PaddingTop>
					</Style>
					<ZIndex>7</ZIndex>
					<CanGrow>true</CanGrow>
					<Value>=Fields!<xsl:value-of select="$varFieldName"/>.Value</Value>
				</Textbox>
			</ReportItems>
		</TableCell>
	</xsl:template>

	<xsl:template match="xs:element" mode="TableColumn">
		<TableColumn>
			<Width>0.75in</Width>
		</TableColumn>
	</xsl:template>

	<xsl:template name="replace-string">
		<xsl:param name="text"/>
		<xsl:param name="from"/>
		<xsl:param name="to"/>
		<xsl:choose>
			<xsl:when test="contains($text, $from)">
				<xsl:variable name="before" select="substring-before($text, $from)"/>
				<xsl:variable name="after" select="substring-after($text, $from)"/>
				<xsl:variable name="prefix" select="concat($before, $to)"/>
				<xsl:value-of select="$before"/>
				<xsl:value-of select="$to"/>
				<xsl:call-template name="replace-string">
					<xsl:with-param name="text" select="$after"/>
					<xsl:with-param name="from" select="$from"/>
					<xsl:with-param name="to" select="$to"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$text"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>