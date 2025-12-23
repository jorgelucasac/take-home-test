import { test, expect } from '@playwright/test';

const LOANS_API_PATTERN = '**/loans**';

const loansOk = [
  { amount: 25000, currentBalance: 18750, applicantName: 'John Doe', status: 'active' },
  { amount: 15000, currentBalance: 0, applicantName: 'Jane Smith', status: 'paid' },
];

test.describe('Loan Management (E2E)', () => {
  test('smoke: abre a home e mostra o título', async ({ page }) => {
    await page.goto('/');
    await expect(page.getByRole('heading', { name: 'Loan Management' })).toBeVisible();
  });

  test('deve mostrar loading inicialmente', async ({ page }) => {
    await page.route(LOANS_API_PATTERN, async (route) => {
      await new Promise(() => { });
    });

    await page.goto('/');

    await expect(page.getByTestId('loading')).toBeVisible();
    await expect(page.getByTestId('error')).toHaveCount(0);
    await expect(page.getByTestId('loans-table')).toHaveCount(0);

    await page.unrouteAll({ behavior: 'ignoreErrors' });
  });


  test('sucesso: renderiza tabela com dados', async ({ page }) => {
    await page.route(LOANS_API_PATTERN, async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(loansOk),
      });
    });

    await page.goto('/');

    await expect(page.getByRole('heading', { name: 'Loan Management' })).toBeVisible();

    await expect(page.getByTestId('error')).toHaveCount(0);
    await expect(page.getByTestId('loans-table')).toBeVisible();

    await expect(page.getByText('John Doe')).toBeVisible();
    await expect(page.getByText('Jane Smith')).toBeVisible();

    await expect(page.getByText('active')).toBeVisible();
    await expect(page.getByText('paid')).toBeVisible();
  });


  test('erro: mostra mensagem e não renderiza tabela', async ({ page }) => {
    await page.route(LOANS_API_PATTERN, async (route) => {
      await route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ message: 'Internal Server Error' }),
      });
    });

    await page.goto('/');

    await expect(page.getByTestId('loading')).toBeHidden();
    await expect(page.getByTestId('error')).toBeVisible();
    await expect(page.getByTestId('error')).toHaveText('Failed to load loans.');

    await expect(page.getByTestId('loans-table')).toHaveCount(0);
  });
});
