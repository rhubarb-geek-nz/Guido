# rhubarb-geek-nz/Guido
Tool for creating a Guid from value and namespace according to RFC 4122 §4.3

Inspired by [Request updating New-Guid to implement the guid functionality found in ARM and Bicep](https://github.com/PowerShell/PowerShell/discussions/24463)

```
PS> $terminalNamespace = '2bde4a90-d05f-401c-9492-e40884ead1d8'
PS> ConvertTo-Guid $terminalNamespace 'Ubuntu' -Encoding Unicode

Guid
----
2c4de342-38b7-51cf-b940-2309a097f518
```
