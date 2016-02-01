import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../Shared/ValidationSummary'

const form = 'login'
const fields = ['login', 'password', 'returnUrl']
const config = { form, fields, validate, }

const Form = ({ handleSubmit, submitting, submitFailed, error, errors, fields: { login, password, }, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Email address, phone number, or username</label>
        <input type="text" className="form-control" placeholder="Email address, phone number, or username" disabled={submitting} {...login} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Password</label>
        <input type="password" className="form-control" placeholder="Password" disabled={submitting} {...password} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Log in</button>
      </div>
    </div>
    <ValidationSummary errors={{ error, ...errors }} visible={submitFailed && !submitting} />
  </form>
)

Form.propTypes = {
  handleSubmit: React.PropTypes.func.isRequired,
  submitting: React.PropTypes.bool.isRequired,
  submitFailed: React.PropTypes.bool.isRequired,
  error: React.PropTypes.string,
  errors: React.PropTypes.object.isRequired,
  fields: React.PropTypes.object.isRequired,
  formName: React.PropTypes.string.isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
