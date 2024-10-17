use AJTarefas
go

if exists (select 1 from sys.sysobjects where name = 'HistoricoTarefas') drop table HistoricoTarefas
go

if exists (select 1 from sys.sysobjects where name = 'ComentariosTarefas') drop table ComentariosTarefas
go

if exists (select 1 from sys.sysobjects where name = 'Tarefas') drop table Tarefas
go

if exists (select 1 from sys.sysobjects where name = 'Projetos') drop table Projetos
go

if exists (select 1 from sys.sysobjects where name = 'Usuarios') drop table Usuarios
go

create table Projetos
(
Id int identity,
NomeProjeto varchar(300) not null,
DescricaoProjeto varchar(max) not null,
DataCriacao datetime not null,
DataInicio datetime null,
DataTermino datetime null,
DataPrevisaoTermino datetime null,
StatusProjeto int not null
)
go

alter table Projetos
add constraint pk_projeto primary key (Id)
go

create table Tarefas
(
Id int identity,
ProjetoId int not null,
Titulo varchar(30) not null,
Descricao varchar(max) not null,
DataCriacao datetime not null,
DataInicio datetime null,
DataPrevistaTermino datetime null,
DataTermino datetime null,
PrioridadeTarefa int null,
StatusTarefa int not null
)
go

alter table Tarefas
add constraint pk_tarefa primary key (Id, ProjetoId)
go

alter table Tarefas
add constraint fk_tarefa_projeto foreign key (ProjetoId) references Projetos(Id)
go

create table Usuarios
(
Id int identity,
Nome varchar(300) not null,
Papel int not null
)
go

alter table Usuarios
add constraint pk_usuario primary key (Id)
go

create table ComentariosTarefas
(
TarefaId int not null,
ProjetoId int not null,
DataHoraCriacao datetime not null,
Sequencia int identity,
UsuarioId int not null,
Comentario varchar(max) not null
)
go

alter table ComentariosTarefas
add constraint pk_comentarios_tarefa primary key (ProjetoId, TarefaId, DataHoraCriacao, Sequencia)
go

alter table ComentariosTarefas
add constraint comentario_tarefa_tarefa foreign key (TarefaId, ProjetoId) references Tarefas(Id, ProjetoId)
go

alter table ComentariosTarefas
add constraint comentarios_tarefa_usuario foreign key (UsuarioId) references Usuarios(Id)
go

create table HistoricoTarefas
(
TarefaId int not null,
ProjetoId int not null,
DataHoraCriacao datetime not null,
Sequencia int identity,
UsuarioId int not null,
Descricao varchar(max) not null,
DataInicio datetime null,
DataPrevistaTermino datetime null,
DataTermino datetime null,
StatusTarefa int not null
)
go

alter table HistoricoTarefas
add constraint pk_historico_tarefas primary key (ProjetoId, TarefaId, DataHoraCriacao, Sequencia)
go

alter table HistoricoTarefas
add constraint fk_historico_tarefa_tarefa foreign key (TarefaId, ProjetoId) references Tarefas(Id, ProjetoId)
go

alter table HistoricoTarefas
add constraint historico_tarefa_usuario foreign key (UsuarioId) references Usuarios(Id)
go



