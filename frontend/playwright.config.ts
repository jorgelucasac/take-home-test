import { defineConfig } from '@playwright/test';

export default defineConfig({
testDir: './e2e',
  testMatch: /.*\.e2e\.spec\.ts/,
  use: {
    baseURL: 'http://localhost:4200',
    headless: true,
    viewport: { width: 1280, height: 720 },
    trace: 'on-first-retry',
  },
  webServer: {
    command: 'npx ng serve --port 4200',
    url: 'http://localhost:4200',
    reuseExistingServer: process.env['CI'] != "true",
    //reuseExistingServer: true,
    timeout: 120_000,
  },
});
