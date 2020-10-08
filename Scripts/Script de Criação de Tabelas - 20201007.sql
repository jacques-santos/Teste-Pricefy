USE [TestePricefy]
GO
--drop table [TbLogImportacao]
--drop table [TbSalesRecord]
--drop table [TbCSVFile]
-- Tabela para registro de arquivos
CREATE TABLE [dbo].[TbCSVFile](
	[IdCSVFile] [int] IDENTITY(1,1) NOT NULL,
	[CaminhoInicial] [varchar](500) NOT NULL,
	[NomeIdentificacao] [varchar](200) NOT NULL,
	[TotalLinhas] [int] NOT NULL,
	[TotalLinhasImportadas] [int] NOT NULL,
	[TotalLinhasComErro] [int] NOT NULL,
	[StatusProcessamento] [bit] NULL,
	[DescricaoProcessamento] [varchar](200) NULL,
	[DataInsercao] [datetime] NOT NULL,	
PRIMARY KEY CLUSTERED 
(
	[IdCSVFile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-- Tabela com Log de Erros de Importação
CREATE TABLE [dbo].[TbLogImportacao](
	[IdLogImportacao] [int] IDENTITY(1,1) NOT NULL,
	[FK_IdCSVFile] [int] NULL,
	[NumLinha] [int] NOT NULL,
	[ErroImportacao] [varchar](5000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLogImportacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--Tabela com registros do CSV de Exemplo / SalesRecords
CREATE TABLE [dbo].[TbSalesRecord](
	[IdSalesRecord] [int] IDENTITY(1,1) NOT NULL,
	[Fk_IdCSVFile] [int] NOT NULL,
	[Region] [varchar](300) NOT NULL,
	[Country] [varchar](300) NOT NULL,
	[Item_Type] [varchar](300) NOT NULL,
	[Sales_Channel] [varchar](300) NOT NULL,
	[Order_Priority] [varchar](300) NOT NULL,
	[Order_Date] [datetime] NOT NULL,
	[Order_ID] [int] NOT NULL,
	[Ship_Date] [datetime] NOT NULL,
	[Units_Sold] [int] NOT NULL,
	[Unit_Price] [decimal](15, 2) NOT NULL,
	[Unit_Cost] [decimal](15, 2) NOT NULL,
	[Total_Revenue] [decimal](15, 2) NOT NULL,
	[Total_Cost] [decimal](15, 2) NOT NULL,
	[Total_Profit] [decimal](15, 2) NOT NULL,
	[NumLinha] [int] NOT NULL,
	[DataInsercao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSalesRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TbCSVFile] ADD  DEFAULT (getdate()) FOR [DataInsercao]
GO
ALTER TABLE [dbo].[TbSalesRecord] ADD  DEFAULT (getdate()) FOR [DataInsercao]
GO
ALTER TABLE [dbo].[TbLogImportacao]  WITH CHECK ADD  CONSTRAINT [fk_logimportacao_csvfile] FOREIGN KEY([FK_IdCSVFile])
REFERENCES [dbo].[TbCSVFile] ([IdCSVFile])
GO
ALTER TABLE [dbo].[TbLogImportacao] CHECK CONSTRAINT [fk_logimportacao_csvfile]
GO
ALTER TABLE [dbo].[TbSalesRecord]  WITH CHECK ADD  CONSTRAINT [Fk_SalesRecord_CSVFile] FOREIGN KEY([Fk_IdCSVFile])
REFERENCES [dbo].[TbCSVFile] ([IdCSVFile])
GO
ALTER TABLE [dbo].[TbSalesRecord] CHECK CONSTRAINT [Fk_SalesRecord_CSVFile]
GO
