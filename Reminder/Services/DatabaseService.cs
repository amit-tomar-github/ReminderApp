using SQLite;
using Reminder.Models;

namespace Reminder.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized;
        private readonly string _dbPath;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);

        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "payments.db");
        }

        private async Task InitAsync()
        {
            if (_initialized) return;
            await _initSemaphore.WaitAsync();
            try
            {
                if (_initialized) return;
                _database = new SQLiteAsyncConnection(_dbPath);
                await _database.CreateTableAsync<Payment>();
                _initialized = true;
            }
            catch (Exception ex)
            {
                // Log or handle DB init error
                throw new Exception($"Database initialization failed: {ex.Message}", ex);
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        public async Task<List<Payment>> GetCurrentMonthPaymentsAsync()
        {
            await InitAsync();
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);
            try
            {
                return await _database.Table<Payment>()
                    .Where(p => p.DueDate >= startOfMonth &&
                                p.DueDate < startOfNextMonth)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle query error
                throw new Exception($"Failed to fetch payments: {ex.Message}", ex);
            }
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            await InitAsync();
            try
            {
                return await _database.Table<Payment>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle query error
                throw new Exception($"Failed to fetch all payments: {ex.Message}", ex);
            }
        }

        public async Task<int> SavePaymentAsync(Payment payment)
        {
            await InitAsync();
            try
            {
                if (payment.Id == 0)
                    return await _database.InsertAsync(payment);
                return await _database.UpdateAsync(payment);
            }
            catch (Exception ex)
            {
                // Log or handle save error
                throw new Exception($"Failed to save payment: {ex.Message}", ex);
            }
        }

        public async Task<int> DeletePaymentAsync(Payment payment)
        {
            await InitAsync();
            try
            {
                return await _database.DeleteAsync(payment);
            }
            catch (Exception ex)
            {
                // Log or handle delete error
                throw new Exception($"Failed to delete payment: {ex.Message}", ex);
            }
        }
    }
}