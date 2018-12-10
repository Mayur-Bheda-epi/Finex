using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Finex.Data.GenericRepository;


namespace Finex.Data.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {

        #region Private member variables...

        private FinexAlmondzEntities _context = null;
        private GenericRepository<Claim> _claimRepository;
        private GenericRepository<IntimationTransaction> _intimationTransactionRepository;
        private GenericRepository<CardTypeMaster> _cardTypeMasterRepository;
        private GenericRepository<DocumentTypeMaster> _documentTypeMasterRepository;
        private GenericRepository<DocumentUpload> _documentUploadRepository;
        private GenericRepository<LossTypeMaster> _lossTypeRepository;
        private GenericRepository<OTP> _otpRepository;
        private GenericRepository<Password> _passwordRepository;
        private GenericRepository<StatusMaster> _statusMasterRepository;
        private GenericRepository<UserMaster> _userMasterRepository;
        private GenericRepository<UserType> _userTypeRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<CardLossPolicyMapping> _cardLossPolicyMapping;
        private GenericRepository<ClaimReverseFeed> _claimReverseFeed;


        private GenericRepository<CountryMaster> _countryMasterRepository;
        private GenericRepository<StateMaster> _stateMasterRepository;
        private GenericRepository<CityMaste> _cityMasterRepository;
        private GenericRepository<PolicyMaster> _policyMasterRepository;
        private GenericRepository<MailTemplate> _mailTemplateRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new FinexAlmondzEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<Claim> ClaimRepository
        {
            get
            {
                if (this._claimRepository == null)
                    this._claimRepository = new GenericRepository<Claim>(_context);
                return _claimRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<IntimationTransaction> IntimationTransactionRepository
        {
            get
            {
                if (this._intimationTransactionRepository == null)
                    this._intimationTransactionRepository = new GenericRepository<IntimationTransaction>(_context);
                return _intimationTransactionRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<ClaimReverseFeed> ClaimReverseFeedRepository
        {
            get
            {
                if (this._claimReverseFeed == null)
                    this._claimReverseFeed = new GenericRepository<ClaimReverseFeed>(_context);
                return _claimReverseFeed;
            }
        }


        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<CardLossPolicyMapping> CardLossPolicyMappingRepository
        {
            get
            {
                if (this._cardLossPolicyMapping == null)
                    this._cardLossPolicyMapping = new GenericRepository<CardLossPolicyMapping>(_context);
                return _cardLossPolicyMapping;
            }
        }


        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<PolicyMaster> PolicyMasterRepository
        {
            get
            {
                if (this._policyMasterRepository == null)
                    this._policyMasterRepository = new GenericRepository<PolicyMaster>(_context);
                return _policyMasterRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for Card type repository.
        /// </summary>
        public GenericRepository<CardTypeMaster> CardTypeMasterRepository
        {
            get
            {
                if (this._cardTypeMasterRepository == null)
                    this._cardTypeMasterRepository = new GenericRepository<CardTypeMaster>(_context);
                return _cardTypeMasterRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Customer repository.
        /// </summary>
        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                    this._customerRepository = new GenericRepository<Customer>(_context);
                return _customerRepository;
            }
        }

        public GenericRepository<DocumentTypeMaster> DocumentTypeMasterRepository
        {
            get
            {
                if (this._documentTypeMasterRepository == null)
                    this._documentTypeMasterRepository = new GenericRepository<DocumentTypeMaster>(_context);
                return _documentTypeMasterRepository;
            }
        }


        public GenericRepository<DocumentUpload> DocumentUploadRepository
        {
            get
            {
                if (this._documentUploadRepository == null)
                    this._documentUploadRepository = new GenericRepository<DocumentUpload>(_context);
                return _documentUploadRepository;
            }
        }

        public GenericRepository<LossTypeMaster> LossTypeMasterRepository
        {
            get
            {
                if (this._lossTypeRepository == null)
                    this._lossTypeRepository = new GenericRepository<LossTypeMaster>(_context);
                return _lossTypeRepository;
            }
        }

        public GenericRepository<OTP> OTPRepository
        {
            get
            {
                if (this._otpRepository == null)
                    this._otpRepository = new GenericRepository<OTP>(_context);
                return _otpRepository;
            }
        }

        public GenericRepository<Password> PasswordRepository
        {
            get
            {
                if (this._passwordRepository == null)
                    this._passwordRepository = new GenericRepository<Password>(_context);
                return _passwordRepository;
            }
        }

        public GenericRepository<StatusMaster> StatusMasterRepository
        {
            get
            {
                if (this._statusMasterRepository == null)
                    this._statusMasterRepository = new GenericRepository<StatusMaster>(_context);
                return _statusMasterRepository;
            }
        }


        public GenericRepository<UserMaster> UserMasterRepository
        {
            get
            {
                if (this._userMasterRepository == null)
                    this._userMasterRepository = new GenericRepository<UserMaster>(_context);
                return _userMasterRepository;
            }
        }

        public GenericRepository<UserType> UserTypeRepository
        {
            get
            {
                if (this._userTypeRepository == null)
                    this._userTypeRepository = new GenericRepository<UserType>(_context);
                return _userTypeRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<CountryMaster> CountryMasterRepository
        {
            get
            {
                if (this._countryMasterRepository == null)
                    this._countryMasterRepository = new GenericRepository<CountryMaster>(_context);
                return _countryMasterRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<StateMaster> StateMasterRepository
        {
            get
            {
                if (this._stateMasterRepository == null)
                    this._stateMasterRepository = new GenericRepository<StateMaster>(_context);
                return _stateMasterRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for Claim repository.
        /// </summary>
        public GenericRepository<CityMaste> CityMasterRepository
        {
            get
            {
                if (this._cityMasterRepository == null)
                    this._cityMasterRepository = new GenericRepository<CityMaste>(_context);
                return _cityMasterRepository;
            }
        }

        public GenericRepository<MailTemplate> MailTemplateRepository
        {
            get
            {
                if (this._mailTemplateRepository == null)
                    this._mailTemplateRepository = new GenericRepository<MailTemplate>(_context);
                return _mailTemplateRepository;
            }
        }
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
