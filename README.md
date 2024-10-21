# Introdução
## Desafio Eclipse Works##
Criar uma API com as funcionalidades e regras abaixo descritas.

	1. Listagem de Projetos - listar todos os projetos do usuário
	2. Visualização de Tarefas - visualizar todas as tarefas de um projeto específico
	3. Criação de Projetos - criar um novo projeto
	4. Criação de Tarefas - adicionar uma nova tarefa a um projeto
	5. Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
	6. Remoção de Tarefas - remover uma tarefa de um projeto
	
	Regras de negócio:
	1. Prioridades de Tarefas:
		○ Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
		○ Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
	2. Restrições de Remoção de Projetos:
		○ Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
		○ Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
	3. Histórico de Atualizações:
		○ Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
		○ O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
	4. Limite de Tarefas por Projeto:
		○ Cada projeto tem um limite máximo de 20 tarefas. Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
	5. Relatórios de Desempenho:
		○ A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias.
		○ Os relatórios devem ser acessíveis apenas por usuários com uma função específica de "gerente".
	6. Comentários nas Tarefas:
		○ Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
		○ Os comentários devem ser registrados no histórico de alterações da tarefa.

##Resumo da solução

Iniciei criando o banco de dados para a API e a estrutura de dados (os scripts de crição estão na aplicação).
Depois criei a estrutura básica da API definido as camadas de apresentação (app), negócio, domais e recursos (infraestrutura).

Cada camada com sua responsabilidade.

	Camada de apresentação - camada de entry point com os controllers 
	Camada de Domain - camada que permeira toda a aplicação e contendo as definições de classes principais.
	Camada de Negócio - Camada que contém as regras de negócios e validações.
	Camada de estrutura - camada contendo os recurusos da aplicação tipo repositorios, gerador de dados e outras recuros que se façam necessários 
	
Após a criação da estrutura basica, iniciei a implementação dos end points e regras de negócio.

Uma vez feito isso, implementei a conexão com a base de dados e realizei uma bateria de testes para garentir as regras de negócios.

Por fim criei o projeto de testes unitário e a implantação da aplicação no docker.

## Melhorias sugeridas 

	- Criar uma funcionalidade para aviso de tarefas e acompanhamento para os usuários e gerentes.
	- As tarefas poderiam estar desacoplada da aplicação usando algum tipo de mensageria.
	
## Como instalar a aplicação no docker

### Instalando o SQL Server 

No projeto, na pasta onde estão os scripts, possui o arquivos sql-docker-compose.yaml
A partir de uma linha de comando ou do terminal do visual studio ir para a pasta onde o mesmo se encontra e executa o comando abaixo:

docker-compose -f sql-docker-compose.yml up -d

Isso irá instalar o sql server 2022 no docker.

Dicas:  Rode o comamndo "docker ps""para saber se o container estar executando corretamente.
		Use o comando "docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" <nome_do_container> para obter o IP"
		Atenção que o firewall pode estar bloqueando a port 1433

para acessar o banco de dados use o comando "docker exec -it <nome_do_container> "bash""

### Instalando a API
1. Execute o Dockerfile através do comando "docker build -t ajtarefasapp ."

Note que o mesmo se encontra na pasta AJTarefas.

Isso fara a instalação do mesmo no container.

2. Após a instalação execute o comando "docker run -d -p 8080:80 --name ajtarefascontainer ajtarefasapp" para iniciar o container.

Use o comando "docker ps" para se certificar que o mesmo está sendo executado.

Agora você já pode usar a API através do link http://localhost:8080.




