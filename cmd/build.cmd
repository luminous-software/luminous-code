@echo off
cls

copy README.md mkdocs\index.md
copy LICENSE mkdocs\license.md
copy .github\CONTRIBUTING.md mkdocs\contributing.md

mkdocs build
