import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

const form = 'register'
const fields = ['emailOrPhone']
const config = { form, fields, validate, }

const Form = ({ handleSubmit, submitting, submitFailed, error, errors, fields: { emailOrPhone }, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Email address or phone number</label>
        <input type="text" name="emailOrPhone" className="form-control" placeholder="Email address or phone number" disabled={submitting} {...emailOrPhone} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Register</button>
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
}

export default reduxForm(config)(Form)
