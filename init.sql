CREATE DATABASE IF NOT EXISTS TODOshka;
USE TODOshka;

CREATE TABLE IF NOT EXISTS tasks_needed(
    id INT AUTO_INCREMENT PRIMARY KEY,
    task VARCHAR(1024) NOT NULL,
    description VARCHAR(4096)
);

CREATE TABLE IF NOT EXISTS tasks_top_priority(
     id INT AUTO_INCREMENT PRIMARY KEY,
     task VARCHAR(1024) NOT NULL,
     description VARCHAR(4096)
);

CREATE TABLE IF NOT EXISTS tasks_in_progress(
     id INT AUTO_INCREMENT PRIMARY KEY,
     task VARCHAR(1024) NOT NULL,
     description VARCHAR(4096)
);

CREATE TABLE IF NOT EXISTS tasks_finished(
     id INT AUTO_INCREMENT PRIMARY KEY,
     task VARCHAR(1024) NOT NULL,
     description VARCHAR(4096)
);