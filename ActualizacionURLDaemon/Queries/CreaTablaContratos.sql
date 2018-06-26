USE [LegalEngineering]
GO

/****** Object:  Table [dbo].[Contratos]    Script Date: 2018-05-10 09:57:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contratos](
	[SEC] [nvarchar](20) NULL,
	[AÑO] [nvarchar](4) NULL,
	[GOBIERNO] [nvarchar](50) NULL,
	[SIGLAS] [nvarchar](50) NULL,
	[DEPENDENCIA] [nvarchar](500) NULL,
	[CLAVEUC] [nvarchar](500) NULL,
	[NOMBRE_DE_LA_UC] [nvarchar](500) NULL,
	[RESPONSABLE] [nvarchar](500) NULL,
	[CODIGO_EXPEDIENTE] [nvarchar](500) NULL,
	[TITULO_EXPEDIENTE] [nvarchar](500) NULL,
	[PLANTILLA_EXPEDIENTE] [nvarchar](500) NULL,
	[NUMERO_PROCEDIMIENTO] [nvarchar](500) NULL,
	[EXP_F_FALLO] [nvarchar](500) NULL,
	[PROC_F_PUBLICACION] [nvarchar](500) NULL,
	[FECHA_APERTURA_PROPOSICIONES] [nvarchar](50) NULL,
	[CARACTER] [nvarchar](500) NULL,
	[TIPO_CONTRATACION] [nvarchar](500) NULL,
	[TIPO_PROCEDIMIENTO] [nvarchar](500) NULL,
	[FORMA_PROCEDIMIENTO] [nvarchar](500) NULL,
	[CODIGO_CONTRATO] [nvarchar](500) NULL,
	[TITULO_CONTRATO] [nvarchar](500) NULL,
	[FECHA_INICIO] [nvarchar](50) NULL,
	[FECHA_FIN] [nvarchar](50) NULL,
	[IMPORTE_CONTRATO] [nvarchar](50) NULL,
	[MONEDA] [nvarchar](50) NULL,
	[ESTATUS_CONTRATO] [nvarchar](50) NULL,
	[ARCHIVADO] [nvarchar](50) NULL,
	[CONVENIO_MODIFICATORIO] [nvarchar](500) NULL,
	[RAMO] [nvarchar](500) NULL,
	[CLAVE_PROGRAMA] [nvarchar](500) NULL,
	[APORTACION_FEDERAL] [nvarchar](500) NULL,
	[FECHA_CELEBRACION] [nvarchar](500) NULL,
	[CONTRATO_MARCO] [nvarchar](50) NULL,
	[IDENTIFICADOR_CM] [nvarchar](500) NULL,
	[COMPRA_CONSOLIDADA] [nvarchar](500) NULL,
	[PLURIANUAL] [nvarchar](50) NULL,
	[CLAVE_CARTERA_SHCP] [nvarchar](500) NULL,
	[ESTRATIFICACION_MUC] [nvarchar](500) NULL,
	[FOLIO_RUPC] [nvarchar](500) NULL,
	[PROVEEDOR_CONTRATISTA] [varchar](500) NULL,
	[ESTRATIFICACION_MPC] [nvarchar](500) NULL,
	[SIGLAS_PAIS] [nvarchar](50) NULL,
	[ESTATUS_EMPRESA] [nvarchar](50) NULL,
	[CUENTA_ADMINISTRADA_POR] [nvarchar](500) NULL,
	[C_EXTERNO] [nvarchar](500) NULL,
	[ORGANISMO] [nvarchar](500) NULL,
	[ANUNCIO] [nvarchar](500) NULL
) ON [PRIMARY]
GO


