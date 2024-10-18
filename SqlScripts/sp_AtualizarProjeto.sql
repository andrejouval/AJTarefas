use AJTarefas
go

if exists(select 1 from sys.sysobjects where name = 'sp_AtualizarProjeto') drop procedure sp_AtualizarProjeto
go

create procedure sp_AtualizarProjeto
(
@projetoId int 
)
as
begin 

	declare @dataInicio as datetime
	declare @dataTermino as datetime
	declare @statusProjeto as int
	declare @dataPrevistaTermino datetime

	select @dataInicio = min(DataInicio), @statusProjeto = max(StatusTarefa), @dataPrevistaTermino = max(dataPrevistaTermino) from Tarefas where ProjetoId = @projetoId

	select @dataTermino = max(DataTermino) from Tarefas where ProjetoId = @projetoId and StatusTarefa = 4

	update Projetos
	set DataInicio = @dataInicio
	, DataTermino = @dataTermino
	, StatusProjeto = @statusProjeto 
	, DataPrevisaoTermino = @dataPrevistaTermino
	where id = @projetoId


end