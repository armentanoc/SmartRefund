// JavaScript source code
import http from 'k6/http';
import { sleep } from 'k6';
import * as config from './config.js';

export const options = {
        stages: [
            { duration: '10m', target: 5 },
            { duration: '1h', target: 10 },
            { duration: '5m', target: 5 },
            { duration: '1m', target: 10 },
        ],
        thresholds: {
            http_req_duration: ['p(95)<600'],
        },
   };


export default function () {
    http.get(config.API_GETALL_URL);
    sleep(1);
}