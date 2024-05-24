
## How To Setup

 - install dotnet 8
 - install postgresql
 - create database `indiveTest`
 - setup `appsettings.json`
 - dotnet ef migrations add init
 - dotnet ef database update


## How to run
 - dotnet watch run / dotnet run

## Current Step of Work
  - [x]  Buatlah api endpoint (POST /register) untuk membuat user baru dimana user bisa memasukkan data nama, email, alamat, no telp, dan juga password untuk disimpan ke database, ketika user mendaftar maka dia akan mendapatkan email berupa kode untuk melakukan verifikasi (bisa gunakan mailtrap untuk development).
  - [x] Buatlah api endpoint (POST /verify/email) untuk verifikasi email dengan body params kode.
  - [ ] Buatlah api endpoint (GET /oauth/{social-media}) untuk melakukan authentikasi oauth menggunakan sosial media google & facebook.
  - [ ] Buatlah api endpoint (GET /oauth/{social-media}/callback) untuk menerima callback dan memberikan jwt token sebagai response. Ketika email dari user belom terdaftar, maka akan didaftarkan sebagai user baru dengan status email sudah diverifikasi (tanpa mengirim email verifikasi).
  - [x] Buatlah api endpoint (POST /login) untuk melakukan manual login dan mendapatkan jwt token.
  - [x] Buatlah api endpoint (GET /dashboard) dengan header jwt token, dan hanya verified user yang bisa mengakses. Di endpoint ini akan menampilkan total users, dan juga list user beserta tgl dibuat & tgl verifikasi email menggunakan pagination.


Karna beberapa kendala pribadi bagian oauth belum tersentuh, jadi baru step login manual saja.