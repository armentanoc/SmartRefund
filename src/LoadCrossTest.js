// JavaScript source code
import * as config from './config.js';
import http from 'k6/http';
import { sleep } from 'k6';



const file = open('sample.txt', 'b');

export const options = {
    stages: [
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<600'],
    },
};

export default function () {
    
    http.get(config.API_GETALL_URL);

    
    const employeeId = Math.floor(Math.random() * 1000) + 1; // Gerando um ID de funcionário aleatório

    const formData = {
        file: http.file(file, 'sample.txt'), // Substitua 'path/to/file.txt' pelo caminho do arquivo desejado
        employeeId: employeeId.toString(),
    };

    const res = http.post(config.API_POST_URL, formData);

    sleep(1);
}