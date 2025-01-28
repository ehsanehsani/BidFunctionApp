import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    vus: 10, // Number of virtual users
    duration: '30s', // Duration of the test
};

export default function () {
    const url = 'http://localhost:7056/api/sendFunction';

    const randomBidderName = `Bidder_${Math.floor(Math.random() * 1000)}`;
    const randomAmount = (Math.random() * 1000).toFixed(2);
    const randomBidTime = new Date().toISOString();

    const payload = JSON.stringify({
        BidderName: randomBidderName,
        Amount: parseFloat(randomAmount),
        BidTime: randomBidTime,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const response = http.post(url, payload, params);

    // Validate the response
    check(response, {
        'status is 201': (r) => r.status === 201,
        'response time < 500ms': (r) => r.timings.duration < 500,
    });

    sleep(1);
}
