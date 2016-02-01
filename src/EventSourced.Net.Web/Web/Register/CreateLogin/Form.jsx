import { reduxForm } from 'redux-form'
import ValidateUsernameField from '../ValidateUsername/FieldConnector'
import validate, { messages } from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

const form = 'redeem'
const fields = ['username', 'password', 'passwordConfirmation', 'correlationId', 'token']
const config = { form, fields, validate, }

const Form = ({ fields: { username, password, passwordConfirmation, token, },
  handleSubmit, asyncValidating, submitting, submitFailed, formName, formKey, error, errors,
  viewData: { purpose, contactValue, }, }) => (
    <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>

      <div className="form-group has-success">
      { purpose === 'CreateUserFromEmail' ?
        <div className="col-md-6">
          <label className="control-label sr-only">Email address:</label>
          <input type="email" name="emailOrPhone" className="form-control" value={contactValue} disabled="disabled" />
          <p className="help-block">You will be able to login using your email address above.</p>
        </div>
        :
        <div className="col-md-6">
          <label className="control-label sr-only">Phone number:</label>
          <input type="tel" name="emailOrPhone" className="form-control" value={phoneNumberFormatted} disabled="disabled" />
          <p className="help-block">You will be able to login using your phone number above.</p>
        </div>
        }
      </div>

      <ValidateUsernameField form={{ asyncValidating, submitting, formName, formKey, errors, }} field={username} />

      <div className="form-group">
        <div className="col-md-6">
          <label className="control-label sr-only">Password</label>
          <input type="password" className="form-control" placeholder="Create a password" disabled={submitting} {...password} />
          <p className="help-block">Must be at least 8 characters long.</p>
        </div>
      </div>

      <div className="form-group">
        <div className="col-md-6">
          <label className="control-label sr-only">Confirm Password</label>
          <input type="password" className="form-control" placeholder="Enter same password as above" disabled={submitting} {...passwordConfirmation} />
          <p className="help-block">Make double sure you typed it correctly.</p>
        </div>
      </div>

      <div className="form-group">
        <div className="col-md-10">
          <button type="submit" className="btn btn-default" disabled={submitting || asyncValidating}>Create login</button>
        </div>
      </div>

      <ValidationSummary errors={{ error, ...errors }} visible={submitFailed && !submitting} />
    </form>
)

Form.propTypes = {
  viewData: React.PropTypes.object.isRequired,
  fields: React.PropTypes.object.isRequired,
  handleSubmit: React.PropTypes.func.isRequired,
  submitting: React.PropTypes.bool.isRequired,
  formName: React.PropTypes.string.isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
