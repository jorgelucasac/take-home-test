import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './src/tests/e2e',
  testMatch: /.*\.(spec|test)\.(ts|js)/,
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
