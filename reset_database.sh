#!/bin/bash
set -xe

cd WebApp
rm BanCobradotas.db
sqlite3 -init BanCobradotas.sql BanCobradotas.db