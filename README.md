# Lenceas.Blog  
http://api.lujiesheng.cn/index.html
个人博客 前后端分离 API接口文档 .NET Core 3.1 API + EF Core + MySql

功能与进度

一、框架模块：

用 服务+接口 的形式封装框架

异步 async/await 开发

接入数据库 ORM 组件 —— EF Core(主要)/SqlSugar(测试用)

目前支持 MySql (后期增加自由切换多种数据库 Sqlite/SqlServer/MySql/PostgreSQL/Oracle)

实现项目启动，自动生成种子数据 

CodeFirst 简化操作,快速迭代开发,专注业务代码逻辑,不必过多关心数据库

2种日志记录，异常/服务操作(后期增加:审计/请求响应/Sql记录)

设计1种 AOP 切面编程，功能涵盖：日志(后期增加:缓存、审计、事务)


二、组件模块：

使用 Swagger 做api文档

使用 AutoFac 做依赖注入容器，并提供批量服务注入

使用 Log4Net 日志框架，集成原生 ILogger 接口做日志记录
