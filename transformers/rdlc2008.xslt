<?xml version="1.0"?>
<!-- Stylesheet for creating ReportViewer RDLC documents -->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"
  xmlns="http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition" 
  xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"  
  exclude-result-prefixes="xsl msxsl xs msdata">

	<xsl:output method="xml"
            encoding="UTF-8"
            indent="yes"/>

	<xsl:variable name="mvarName" select="/xs:schema/@Name"/>
    <xsl:variable name="mSchemaPath" select="/xs:schema/@FullPath"/>
	<xsl:variable name="mdsName" select="/xs:schema/xs:element/@name" />
	<xsl:variable name="mtblName" select="/xs:schema/xs:element/xs:complexType/xs:choice/xs:element/@name" />
	<xsl:variable name="mvarFontSize">8pt</xsl:variable>
	<xsl:variable name="mvarFontWeight">500</xsl:variable>
	<xsl:variable name="mvarFontWeightBold">Bold</xsl:variable>


	<xsl:template match="/">
		<xsl:apply-templates select="/xs:schema/xs:element/xs:complexType/xs:choice/xs:element/xs:complexType/xs:sequence">
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="xs:sequence">
		<Report>
			<xsl:call-template name="BuildDataSource">
			</xsl:call-template>
			<xsl:call-template name="BuildDataSet">
			</xsl:call-template>
			<Body>
				<ReportItems>
					<Tablix Name="table1">
						<TablixBody>
							<TablixColumns>
								<xsl:apply-templates select="xs:element" mode="TablixBodyColumn" />
							</TablixColumns>
							<TablixRows>
								<TablixRow>
									<Height>0.25in</Height>
									<TablixCells>
										<xsl:apply-templates select="xs:element" mode="TablixHeaderRow" />
									</TablixCells>
								</TablixRow>
								<TablixRow>
									<Height>0.25in</Height>
									<TablixCells>
										<xsl:apply-templates select="xs:element" mode="TablixDataRow" />
									</TablixCells>
								</TablixRow>
							</TablixRows>
						</TablixBody>
						<TablixColumnHierarchy>
							<TablixMembers>
								<xsl:apply-templates select="xs:element" mode="TablixColumn"/>
							</TablixMembers>
						</TablixColumnHierarchy>
						<TablixRowHierarchy>
							<TablixMembers>
								<TablixMember>
									<KeepWithGroup>After</KeepWithGroup>
									<KeepTogether>true</KeepTogether>
								</TablixMember>
								<TablixMember>
									<Group Name="table1_Details_Group">
										<DataElementName>Detail</DataElementName>
									</Group>
									<TablixMembers>
										<TablixMember />
									</TablixMembers>
									<DataElementName>Detail_Collection</DataElementName>
									<DataElementOutput>Output</DataElementOutput>
									<KeepTogether>true</KeepTogether>
								</TablixMember>
							</TablixMembers>
						</TablixRowHierarchy>
						<!--<Header>
							<TableRows>
								<TableRow>
									<Height>0.25in</Height>
									<TableCells>

										<xsl:apply-templates select="xs:element" mode="HeaderTableCell">
										</xsl:apply-templates>

									</TableCells>
								</TableRow>
							</TableRows>
						</Header>-->
						<!--<TableColumns>
							<xsl:apply-templates select="xs:element" mode="TableColumn">
							</xsl:apply-templates>
						</TableColumns>-->
						<!--<ZIndex>1</ZIndex>
						<DataSetName>
							<xsl:value-of select="$mvarName" />
						</DataSetName>
						<Top>0.5in</Top>
						<Height>0.50in</Height>-->
						<DataSetName>
							<xsl:value-of select="$mvarName" />
						</DataSetName>
						<Top>0.5in</Top>
						<Height>0.5in</Height>
						<Width>5.25in</Width>
						<Style />
					</Tablix>
				</ReportItems>
				<Height>1in</Height>
				<Style />
			</Body>
			<Width>6.5in</Width>
			<Page>
				<LeftMargin>1in</LeftMargin>
				<RightMargin>1in</RightMargin>
				<TopMargin>1in</TopMargin>
				<BottomMargin>1in</BottomMargin>
				<Style />
			</Page>
			<Language>en-US</Language>
			<ConsumeContainerWhitespace>true</ConsumeContainerWhitespace>
			<rd:ReportID>7358b654-3ca3-44a0-8677-efe0a55c7c45</rd:ReportID>
			<rd:ReportUnitType>Inch</rd:ReportUnitType>
			<!--<InteractiveHeight>11in</InteractiveHeight>
			<rd:DrawGrid>true</rd:DrawGrid>
			<InteractiveWidth>8.5in</InteractiveWidth>
			<rd:SnapToGrid>true</rd:SnapToGrid>
			<RightMargin>1in</RightMargin>
			<LeftMargin>1in</LeftMargin>
			<BottomMargin>1in</BottomMargin>
			<TopMargin>1in</TopMargin>
			<Language>en-US</Language>
			<rd:ReportID>7358b654-3ca3-44a0-8677-efe0a55c7c45</rd:ReportID>

			<Width>6.5in</Width>-->
		</Report>
	</xsl:template>

	<xsl:template name="BuildDataSource">
		<DataSources>
			<DataSource Name="DummyDataSource">
				<ConnectionProperties>
					<DataProvider>System.Data.DataSet</DataProvider>
					<ConnectString>/* Local Connection */</ConnectString>
				</ConnectionProperties>
				<rd:DataSourceID>84635ff8-d177-4a25-9aa5-5a921652c79c</rd:DataSourceID>
			</DataSource>
		</DataSources>
	</xsl:template>

	<xsl:template name="BuildDataSet">
		<DataSets>
			<DataSet Name="{$mvarName}">
				<Fields>
					<xsl:apply-templates select="xs:element" mode="Field">
					</xsl:apply-templates>
				</Fields>
				<Query>
					<DataSourceName>DummyDataSource</DataSourceName>
					<CommandText>/* Local Query */</CommandText>
					<!--<rd:UseGenericDesigner>true</rd:UseGenericDesigner>-->
				</Query>
				<rd:DataSetInfo>
					<rd:DataSetName><xsl:value-of select="$mdsName"/></rd:DataSetName>
					<rd:TableName><xsl:value-of select="$mtblName"/></rd:TableName>
                    <rd:SchemaPath><xsl:value-of select="$mSchemaPath"/></rd:SchemaPath>
				</rd:DataSetInfo>
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
				<xsl:when test="@type='xs:decimal'">System.Decimal</xsl:when>
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

	<xsl:template match="xs:element" mode="TablixBodyColumn">
		<TablixColumn>
			<Width>0.75in</Width>
		</TablixColumn>
	</xsl:template>

	<xsl:template match="xs:element" mode="TablixColumn">
		<TablixMember />
	</xsl:template>

	<xsl:template match="xs:element" mode="TablixHeaderRow">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>
		<TablixCell>
			<CellContents>
				<Textbox Name="textbox{position()}">
					<CanGrow>true</CanGrow>
                    <CanShrink>true</CanShrink>
					<KeepTogether>true</KeepTogether>
					<Paragraphs>
						<Paragraph>
							<TextRuns>
								<TextRun>
									<Value><xsl:value-of select="$varFieldName"/></Value>
									<Style>
										<FontWeight><xsl:value-of select="$mvarFontWeightBold"/></FontWeight>
									</Style>
								</TextRun>
							</TextRuns>
							<Style>
								<TextAlign>Center</TextAlign>
							</Style>
						</Paragraph>
					</Paragraphs>
					<rd:DefaultName>textbox<xsl:value-of select="position()"/></rd:DefaultName>
					<ZIndex>7</ZIndex>
					<Style>
						<PaddingLeft>2pt</PaddingLeft>
						<PaddingRight>2pt</PaddingRight>
						<PaddingTop>2pt</PaddingTop>
						<PaddingBottom>2pt</PaddingBottom>
					</Style>
				</Textbox>
			</CellContents>
		</TablixCell>
	</xsl:template>

	<xsl:template match="xs:element" mode="HeaderTableCell">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>

		<TableCell>
			<ReportItems>
				<Textbox Name="textbox{position()}">
					<rd:DefaultName>
						textbox<xsl:value-of select="position()"/>
					</rd:DefaultName>
					<Value>
						<xsl:value-of select="$varFieldName"/>
					</Value>
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

	<xsl:template match="xs:element" mode="TablixDataRow">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>
		<TablixCell>
			<CellContents>
				<Textbox Name="{$varFieldName}">
					<CanGrow>true</CanGrow>
					<KeepTogether>true</KeepTogether>
					<Paragraphs>
						<Paragraph>
							<TextRuns>
								<TextRun>
									<Value>=Fields!<xsl:value-of select="$varFieldName"/>.Value</Value>
									<Style />
								</TextRun>
							</TextRuns>
							<Style>
								<TextAlign>Left</TextAlign>
							</Style>
						</Paragraph>
					</Paragraphs>
					<rd:DefaultName><xsl:value-of select="$varFieldName"/></rd:DefaultName>
					<ZIndex>7</ZIndex>
					<Style>
						<PaddingLeft>2pt</PaddingLeft>
						<PaddingRight>2pt</PaddingRight>
						<PaddingTop>2pt</PaddingTop>
						<PaddingBottom>2pt</PaddingBottom>
					</Style>
				</Textbox>
			</CellContents>
		</TablixCell>
	</xsl:template>

	<xsl:template match="xs:element" mode="DetailTableCell">
		<xsl:variable name="varFieldName">
			<xsl:value-of select="@name" />
		</xsl:variable>

		<TableCell>
			<ReportItems>
				<Textbox Name="{$varFieldName}">
					<rd:DefaultName>
						<xsl:value-of select="$varFieldName"/>
					</rd:DefaultName>
					<Style>
						<TextAlign>Left</TextAlign>
						<PaddingLeft>2pt</PaddingLeft>
						<PaddingBottom>2pt</PaddingBottom>
						<PaddingRight>2pt</PaddingRight>
						<PaddingTop>2pt</PaddingTop>
					</Style>
					<ZIndex>7</ZIndex>
					<CanGrow>true</CanGrow>
					<Value>
						=Fields!<xsl:value-of select="$varFieldName"/>.Value
					</Value>
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